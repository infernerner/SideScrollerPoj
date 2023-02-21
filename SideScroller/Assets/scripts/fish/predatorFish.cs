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
    public Transform turnTransform;
    public GameObject target;
    private void Start()
    {
        myRB = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        Debug.Log(Quaternion.RotateTowards(transform.rotation,turnTransform.rotation,180));
        turnTransform.LookAt(target.transform, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, turnTransform.rotation, 180);
        //if (Physics.OverlapSphere(transform.position,10) != null)
        //{
        //
        //    if (myEyes.collider.GetComponent<predatorFish>().FishTier <= FishTier)
        //    {
        //        myRB.AddForce(transform.up * swimSpeed);
        //        
        //    }
        //
        //}
        //else
        //{
        //    myRB.AddForce(transform.up * swimSpeed);
        //    myRB.AddTorque(transform.forward * turnSpeed * Random.Range(-1f, 1f) * Mathf.Sin(Time.fixedTime));
        //}
        //
        //
    }
}
