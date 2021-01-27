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

    bool isJump = false;
    bool isDrag = false;
    bool throwReady = false;
    bool transToDragBefore;

    Vector3 moveVec;
    Rigidbody rigid;
    Animator anim;

    public GameObject[] items;
    public CapsuleCollider col;
    GameObject nearItem;
    GameObject nearDrag;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && !isJump/*!ani.GetBool("isJumping")*/)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            //ani.SetBool("isJumping", true);
            isJump = true;
        }

        if (Input.GetButtonDown("Interation") && nearItem != null)
        {
            if(nearItem.tag == "Item")
            {
                Item item = nearItem.GetComponent<Item>();
                items[item.value].SetActive(true);

                Destroy(nearItem);
                throwReady = true;
            }
        }

        if (Input.GetButtonDown("Interation") && nearDrag != null)
        {
            

            if (isDrag == false)
            {
                transToDragBefore = transform.position.z < nearDrag.transform.position.z ? true : false;

                if (transToDragBefore)
                {
                    transform.position = new Vector3(nearDrag.transform.position.x, transform.position.y+0.375f, nearDrag.transform.position.z - 2f);
                    transform.LookAt(nearDrag.transform);
                    //transform.position += new Vector3(0.5f, 0f, 0f);
                }
                else
                {
                    transform.position = new Vector3(nearDrag.transform.position.x, transform.position.y+0.375f, nearDrag.transform.position.z + 2f);
                    transform.LookAt(nearDrag.transform);
                    //transform.position += new Vector3(-0.5f, 0f, 0f);
                }
                col.center = new Vector3(0f, 1.5f, 0f);
                anim.SetTrigger("DragStart");
                isDrag = true;
            }
            else
            {
                col.center = new Vector3(0f, 1.37f, 0f);
                anim.SetBool("DragExit", true);
                isDrag = false;
            }
        }
    }

    void FixedUpdate()
    {
        vAxis = Input.GetAxisRaw("Vertical");
        hAxis = Input.GetAxisRaw("Horizontal");
        
        moveVec.Set(-vAxis, 0f, hAxis);
        moveVec = moveVec.normalized;

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

        Vector3 move = moveVec * moveSpeed * Time.deltaTime;

        if (vAxis != 0 || hAxis != 0)
            anim.SetBool("isWalk", true);
        else
            anim.SetBool("isWalk", false);

        if (Input.GetKey(KeyCode.LeftShift))
            anim.SetBool("isRun", true);
        else
            anim.SetBool("isRun", false);

        if(anim.GetBool("isRun"))
            rigid.MovePosition(transform.position + (move * 2f));
        else
            rigid.MovePosition(transform.position + move);


        if (move != Vector3.zero)
        {
            Quaternion rotatePlayer = Quaternion.LookRotation(move);
            rigid.rotation = Quaternion.Slerp(rigid.rotation, rotatePlayer, rotateSpeed * Time.deltaTime);
        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Platform")
            isJump = false;
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
