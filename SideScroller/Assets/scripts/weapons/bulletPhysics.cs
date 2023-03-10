using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletPhysics : MonoBehaviour
{
    private Rigidbody myRB;
    private RaycastHit water;

    private void Start()
    {
        myRB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (myRB.velocity.magnitude > 2f)
        transform.LookAt(myRB.velocity + transform.position, Vector3.up);
        Physics.Raycast(transform.position, Vector3.forward, out water);
        if (water.collider == null)
        {
            myRB.drag = 1f;
            myRB.angularDrag = 1f;
        }
        else if (water.collider.tag != "Water")
        {
            myRB.drag = 1f;
            myRB.angularDrag = 1f;
        }
        else if (water.collider.tag == "Water")
        {
            myRB.drag = 2f;
            myRB.angularDrag = 2f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<predatorFish>())
        {
            collision.gameObject.GetComponent<predatorFish>().dead = true;
        }
    }
}
