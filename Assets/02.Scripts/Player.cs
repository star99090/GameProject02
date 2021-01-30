using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float rotateSpeed;
    public float moveSpeed;
    public float jumpPower;
    private float vAxis;
    private float hAxis;

    bool isAlive = true;
    bool isJump = false;
    bool isDrag = false;
    bool throwReady = false;
    bool transToDragBefore;
    bool isDraggingFrag = false;

    public CapsuleCollider col;
    Rigidbody rigid;
    Animator anim;
    Vector3 moveVec;

    public GameObject[] items;
    GameObject nearItem;
    GameObject nearDrag;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isAlive)
        {
            // Jump
            if (Input.GetButtonDown("Jump") && !isJump /*!anim.GetBool("isJump")*/)
            {
                isJump = true;
                //anim.SetBool("isJump", true);
                rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }

            // Item 획득
            if (Input.GetButtonDown("Interation") && nearItem != null)
            {
                if (nearItem.tag == "Item")
                {
                    Item item = nearItem.GetComponent<Item>();
                    items[item.value].SetActive(true);

                    Destroy(nearItem);
                    throwReady = true;
                }
            }

            // Drag 자세 시작 및 중단
            if (Input.GetButtonDown("Interation") && nearDrag != null)
            {
                // Drag 자세 취하기
                if (isDrag == false)
                {
                    transToDragBefore = transform.position.z < nearDrag.transform.position.z ? true : false;
                    isAlive = false;

                    // DragObject보다 왼쪽에 있을 때
                    if (transToDragBefore)
                    {
                        transform.position = nearDrag.transform.position + Vector3.back * 1.9f;
                        transform.LookAt(nearDrag.transform);
                        transform.position += new Vector3(0.75f, -0.75f, 0f);
                    }

                    // DragObject보다 오른쪽에 있을 때
                    else
                    {
                        transform.position = nearDrag.transform.position + Vector3.forward * 1.9f;
                        transform.LookAt(nearDrag.transform);
                        transform.position += new Vector3(-0.75f, -0.75f, 0f);
                    }
                    nearDrag.GetComponent<Rigidbody>().isKinematic = false;
                    col.center = new Vector3(0f, 1.55f, 0f);
                    anim.SetTrigger("DragStart");
                    Invoke("DragSet", 1.305f);
                }

                // Drag 자세 취소하기
                else
                {
                    isAlive = false;
                    nearDrag.GetComponent<Rigidbody>().isKinematic = true;
                    col.center = new Vector3(0f, 1.37f, 0f);
                    anim.SetBool("DragExit", true);
                    anim.SetBool("Dragging", false);
                    Invoke("DragExit", 1.3f);
                    isDraggingFrag = false;
                    anim.speed = 1f;
                }
            }
        }        
    }

    void FixedUpdate()
    {
        vAxis = Input.GetAxisRaw("Vertical");
        hAxis = Input.GetAxisRaw("Horizontal");
        
        moveVec.Set(-vAxis, 0f, hAxis);
        moveVec = moveVec.normalized;
        
        // Item 투척
        if (throwReady)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                throwReady = false;
                items[0].SetActive(false);
                GameObject obj = Instantiate(items[0], transform.position + moveVec + Vector3.up, Quaternion.identity);
                obj.SetActive(true);
                obj.GetComponent<Rigidbody>().isKinematic = false;
                obj.GetComponent<Rigidbody>().AddForce(transform.forward * 15f, ForceMode.Impulse);
            }
        }

        // Idle 상태의 이동, 회전 및 애니메이션 관리
        if (isDrag == false)
        {
            Vector3 move = moveVec * moveSpeed * Time.deltaTime;

            if (vAxis != 0 || hAxis != 0) anim.SetBool("isWalk", true);
            else anim.SetBool("isWalk", false);

            if (Input.GetKey(KeyCode.LeftShift)) anim.SetBool("isRun", true);
            else anim.SetBool("isRun", false);

            // Player 이동 및 회전
            if (isAlive)
            {
                // Player 이동
                if (anim.GetBool("isRun") /*&& moveReady*/) rigid.MovePosition(transform.position + (move * 1.8f));
                else /*if(!anim.GetBool("isRun") && moveReady)*/ rigid.MovePosition(transform.position + move);

                // Player 회전
                if (moveVec != Vector3.zero)
                {
                    Quaternion rotatePlayer = Quaternion.LookRotation(moveVec);
                    rigid.rotation = Quaternion.Slerp(rigid.rotation, rotatePlayer, rotateSpeed * Time.deltaTime);
                }
            }
        }

        // Drag 상태의 이동, 회전 및 애니메이션 관리
        else
        {
            moveVec.Set(0f, 0f, hAxis);
            Vector3 move = moveVec.normalized * moveSpeed * Time.deltaTime;

            // DragObject보다 왼쪽에 있을 때
            if (transToDragBefore)
            {
                if (isAlive)
                {
                    // DragObject와 함께 Player 좌측 이동
                    if (hAxis < 0)
                    {
                        // Drag 자세 취하고 Dragging하려고 할 때 1번만 위치 조정
                        if (isDraggingFrag == false)
                        {
                            col.center = new Vector3(0f, 1.37f, 0f);
                            isDraggingFrag = true;
                        }

                        anim.speed = 1f;
                        anim.SetBool("Dragging", true);
                        rigid.MovePosition(transform.position + (move * 0.3f));
                        if ((nearDrag.transform.position.z - transform.position.z) > 1.445f)
                            nearDrag.GetComponent<Rigidbody>().MovePosition(nearDrag.transform.position + move * 0.37f);
                    }

                    // Pause
                    else
                    {
                        anim.speed = 0f;
                    }
                }
            }

            // DragObject보다 오른쪽에 있을 때
            else
            {
                if (isAlive)
                {
                    // DragObject와 함께 Player 우측 이동
                    if (hAxis > 0)
                    {
                        // Drag 자세 취하고 Dragging하려고 할 때 1번만 위치 조정
                        if (isDraggingFrag == false)
                        {
                            col.center = new Vector3(0f, 1.37f, 0f);
                            isDraggingFrag = true;
                        }

                        anim.speed = 1f;
                        anim.SetBool("Dragging", true);
                        rigid.MovePosition(transform.position + (move * 0.3f));
                        if ((transform.position.z - nearDrag.transform.position.z) > 1.445f)
                            nearDrag.GetComponent<Rigidbody>().MovePosition(nearDrag.transform.position + move * 0.37f);
                    }

                    // Pause
                    else
                    {
                        anim.speed = 0f;
                    }
                }
            }
        }
    }

    // Drag 자세 취하는 셋팅
    void DragSet()
    {
        isDrag = true;
        isAlive = true;
    }

    // Drag 자세 벗어나는 셋팅
    void DragExit()
    {
        isDrag = false;
        isAlive = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "Drag")
            isJump = false; //anim.SetBool("isJump", false);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Item")
        {
            nearItem = other.gameObject;
        }

        if(other.tag == "Drag")
        {
            nearDrag = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Item")
        {
            nearItem = null;
        }

        if (other.tag == "Drag")
        {
            nearDrag = null;
        }
    }
}
