using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    bool isFirst = true;
    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rigid.useGravity == true)
        {
            rigid.velocity = new Vector3(0f, -250f, 0f)*Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && isFirst == true)
        {
            Debug.Log("Damaged");
            isFirst = false;
            rigid.useGravity = false;
        }

        if(collision.gameObject.tag == "Platform")
        {
            rigid.useGravity = false;
        }
    }
}
