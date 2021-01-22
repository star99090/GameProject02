using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    bool isFirst = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && isFirst == true)
        {
            Debug.Log("Damaged");
            isFirst = false;
        }
    }
}
