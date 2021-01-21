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

    Vector3 moveVec;
    Rigidbody rigid;
    Animator anim;

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
    }

    void FixedUpdate()
    {
        vAxis = Input.GetAxisRaw("Vertical");
        hAxis = Input.GetAxisRaw("Horizontal");
        
        moveVec.Set(-vAxis, 0f, hAxis);
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
}
