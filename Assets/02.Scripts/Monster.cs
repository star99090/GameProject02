using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public GameObject target;
    private bool isTrace;
    private bool isStart;
    
    private void Update()
    {
        if (isStart)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z), Time.deltaTime);
            if (Vector3.Distance(target.transform.position, transform.position) <= 0.5f)
            {
                isTrace = false;
            }
            else
            {
                isTrace = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isStart = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            isStart = false;
        }
    }
}
