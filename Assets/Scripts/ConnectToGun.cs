using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectToGun : MonoBehaviour
{
    public GameObject gunPoint;
    public HingeJoint2D hinge;
    // Start is called before the first frame update
    void Start()
    {
        gunPoint = GameObject.FindWithTag("Gun Point");
        hinge.connectedBody = gunPoint.GetComponent<Rigidbody2D>();
        transform.SetParent(gunPoint.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
