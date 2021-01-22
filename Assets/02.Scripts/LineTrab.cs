using UnityEngine;

public class LineTrab : MonoBehaviour
{
    public GameObject trab;
    Rigidbody rigid;

    private void Awake()
    {
        rigid = trab.gameObject.GetComponent<Rigidbody>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        rigid.isKinematic = false;
        Destroy(this.gameObject);
    }
}
