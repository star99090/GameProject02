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
    bool throwReady = false;

    Vector3 moveVec;
    Rigidbody rigid;
    Animator anim;

    public GameObject[] items;
    GameObject nearObject;

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

        if (Input.GetButtonDown("Interation") && nearObject != null)
        {
            if(nearObject.tag == "Item")
            {
                Item item = nearObject.GetComponent<Item>();
                items[item.value].SetActive(true);

                Destroy(nearObject);
                throwReady = true;
            }
        }
    }

    void FixedUpdate()
    {
        vAxis = Input.GetAxisRaw("Vertical");
        hAxis = Input.GetAxisRaw("Horizontal");
        
        moveVec.Set(-vAxis, 0f, hAxis);

        if (throwReady)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                throwReady = false;
                items[0].SetActive(false);
                GameObject obj = Instantiate(items[0], transform.position + moveVec + Vector3.up, Quaternion.identity);
                obj.GetComponent<Rigidbody>().AddForce(Vector3.forward * 5f);
            }
        }

        moveVec = moveVec.normalized * moveSpeed * Time.deltaTime;

        if (vAxis != 0 || hAxis != 0)
            anim.SetBool("isWalk", true);
        else
            anim.SetBool("isWalk", false);

        if (Input.GetKey(KeyCode.LeftShift))
            anim.SetBool("isRun", true);
        else
            anim.SetBool("isRun", false);

        if(anim.GetBool("isRun"))
            rigid.MovePosition(transform.position + (moveVec * 2f));
        else
            rigid.MovePosition(transform.position + moveVec);


        if (moveVec != Vector3.zero)
        {
            Quaternion rotatePlayer = Quaternion.LookRotation(moveVec);
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
            nearObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Item")
        {
            nearObject = null;
        }
    }
}
