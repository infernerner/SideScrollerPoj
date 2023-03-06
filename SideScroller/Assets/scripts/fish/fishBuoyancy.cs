using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishBuoyancy : MonoBehaviour
{
    private Rigidbody myRB;
    private RaycastHit water;
    public int floaters = 1;

    void Start()
    {
        myRB = transform.parent.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(transform.position, Vector3.forward, out water);
        if (water.collider != null)
        {
            if (water.collider.tag == "Water")
            {
                myRB.AddForceAtPosition(Vector3.up * 2, transform.position);
            }
        }
        else
        {
            myRB.AddForceAtPosition(Physics.gravity / floaters, transform.position);
        }
    }
}
