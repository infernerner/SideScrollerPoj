using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class predatorFish : MonoBehaviour
{
    private Rigidbody myRB;
    private RaycastHit myEyes;

    public int FishTier = 1;
    public float swimSpeed = 200;
    public float turnSpeed = 100;
    private void Start()
    {
        myRB = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if (Physics.OverlapSphere(transform.position,10) != null)
        {
            Debug.Log("a");
            if (myEyes.collider.GetComponent<predatorFish>().FishTier <= FishTier)
            {
                myRB.AddForce(transform.up * swimSpeed);
                myRB.AddTorque(transform.forward * turnSpeed * Vector3.Angle(transform.position, myEyes.collider.transform.position) * Mathf.Sin(Time.fixedTime));
            }

        }
        else
        {
            myRB.AddForce(transform.up * swimSpeed);
            myRB.AddTorque(transform.forward * turnSpeed * Random.Range(-1f, 1f) * Mathf.Sin(Time.fixedTime));
        }


    }
}
