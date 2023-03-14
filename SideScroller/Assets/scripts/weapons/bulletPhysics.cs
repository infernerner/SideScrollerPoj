using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class bulletPhysics : MonoBehaviour
{
    private Collider myCollider;
    public GameObject myPlayer;
    private Rigidbody myRB;
    private RaycastHit water;

    private void Start()
    {
        myCollider = GetComponent<Collider>();
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
            collision.gameObject.GetComponent<predatorFish>().harpooned = true;
            myPlayer.GetComponent<SpringJoint>().connectedBody = collision.gameObject.GetComponent<Rigidbody>();
            transform.parent = collision.gameObject.transform;
            myRB.isKinematic = true;
            myCollider.enabled = false;

        }
        if (collision.gameObject.GetComponent<weaponController>())
        {
            collision.gameObject.GetComponent<weaponController>().readyshot = true;
            collision.gameObject.GetComponent<weaponController>().weapon.active = true;
            myPlayer.GetComponent<SpringJoint>().connectedBody = collision.gameObject.GetComponent<Rigidbody>();
            Destroy(gameObject);
        }
        else
        {
            transform.parent = collision.gameObject.transform;
            myRB.isKinematic = true;
        }

    }
}
