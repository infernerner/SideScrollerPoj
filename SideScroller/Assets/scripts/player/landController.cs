using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class landController : playerController
{
    private swimController mySC;

    public float moveSpeed = 5;

    public float jumpSpeed = 20000;
    public int jumps = 2;
    public float jumpCooldown;

    void Start()
    {
        mySC = GetComponent<swimController>();
        myRB = GetComponent<Rigidbody>();
        myCC = GetComponent<CapsuleCollider>();
        myPI = GetComponent<PlayerInput>();
    }

    void FixedUpdate()
    {
        InteractText();
        transform.position += transform.right * moveVector.x * Time.deltaTime * moveSpeed;

        if (Physics.Raycast(transform.position, -transform.up, out groundRay, 1))
        {
            jumps = 2;
            jumpCooldown = -1;
        }

        if (jump > 0 && jumpCooldown < 0 && jumps > 0)
        {
            jumps--;
            jumpCooldown = 1;
            myRB.AddForce(transform.up * jump * Time.deltaTime * jumpSpeed);
        }

        jumpCooldown -= Time.deltaTime;
        if (Physics.Raycast(transform.position, transform.forward, out interactRay, 1))
        {
            if (interactRay.collider.CompareTag("Ladder"))
            {
                myRB.useGravity = false;
                transform.position += transform.up * moveVector.y * Time.deltaTime * moveSpeed;
                myRB.velocity = Vector3.zero;
            }
        }
        else
        {
            myRB.useGravity = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag == "Water")
        {
            myRB.drag = 1f;
            myRB.useGravity = false;
            myRB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
            mySC.enabled = true;
            this.enabled = false;
        }
    }
}
