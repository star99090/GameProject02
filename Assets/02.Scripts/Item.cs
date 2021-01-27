using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int value;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            GetComponent<Light>().enabled = true;
        }
        if(collision.gameObject.tag == "LineTrab")
        {
            Destroy(this.gameObject);
        }
    }
}
