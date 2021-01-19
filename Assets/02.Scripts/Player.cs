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

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") /*&& !ani.GetBool("isJumping")*/)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            //ani.SetBool("isJumping", true);
            isJump = !isJump;
        }
    }

    void FixedUpdate()
    {
        vAxis = Input.GetAxisRaw("Vertical");
        hAxis = Input.GetAxisRaw("Horizontal");

        moveVec.Set(-vAxis, 0f, hAxis);
        moveVec = moveVec.normalized * moveSpeed * Time.deltaTime;

        //해당 위치로 이동
        rigid.MovePosition(transform.position + moveVec);

        //보간을 이용한 회전
        Quaternion rotatePlayer = Quaternion.LookRotation(moveVec);
        rigid.rotation = Quaternion.Slerp(rigid.rotation, rotatePlayer, rotateSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Platform")
            isJump = !isJump;
    }
}
