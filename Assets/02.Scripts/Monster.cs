using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public GameObject target;
    private NavMeshAgent nav;
    private bool isTrace;
    private bool isStart;

    private void Awake()
    {
        nav = this.gameObject.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (isStart)
        {
            nav.isStopped = false;
            transform.LookAt(target.transform.position);
            nav.SetDestination(target.transform.position);
        }
        else
        {
            nav.isStopped = true;
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
