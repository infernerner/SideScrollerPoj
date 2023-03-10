using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class swimController : playerController
{
    private landController myLC;

    void Start()
    {
        myLC = GetComponent<landController>();
        myRB = GetComponent<Rigidbody>();
        myCC = GetComponent<CapsuleCollider>();
        myPI = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        if (moveVector != Vector3.zero)
        {
           //myRB.AddTorque(0, 0, moveVector.x * -120 * Time.deltaTime);
           //myRB.AddRelativeForce(0, moveVector.y * 300 * Time.deltaTime, 0);

                myRB.AddForce(moveVector * 300 * Time.deltaTime);
                transform.LookAt(transform.position + moveVector);
                transform.Rotate(90, 0, 0);
            
        }
        InteractText();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.tag == "Water")
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            myRB.drag = 0f;
            myRB.useGravity = true;
            myRB.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
            myLC.enabled = true;
            this.enabled = false;
        }
    }
}
