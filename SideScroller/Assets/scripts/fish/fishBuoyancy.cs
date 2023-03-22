using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishBuoyancy : MonoBehaviour
{
    private Rigidbody myRB;
    public int inWater = 0;
    public int floaters = 1;
    public RaycastHit[] waterCheck;

    void Start()
    {
        myRB = transform.parent.GetComponent<Rigidbody>();
    }

    public void dead()
    {
        waterCheck = Physics.RaycastAll(transform.position, Vector3.forward , 5);
        Debug.DrawRay(transform.position, Vector3.forward *5);
        Debug.Log(waterCheck.Length);
        inWater = 0;
        if (waterCheck.Length > -1)
        {
            inWater = 0;
            Debug.Log("failed");
        }
        else
            foreach (RaycastHit hit in waterCheck)
            {
                Debug.Log(hit.collider.name);
                Debug.DrawRay(transform.position, Vector3.forward * hit.distance);
                if (hit.collider.tag == "Water") inWater = +1;
            }
        if (inWater > 0  ) myRB.AddForceAtPosition(Vector3.up * 2, transform.position);
        else myRB.AddForceAtPosition(Physics.gravity / floaters, transform.position);

    }   
}
