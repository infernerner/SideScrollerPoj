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
        if (moveVector.x != 0)
        {
            transform.position += transform.right * moveVector.x * Time.deltaTime * moveSpeed;
            animator.SetBool("move", true);
            if (moveVector.x > 0)
            myAvatar.transform.localScale = new Vector3( 1f,1f,-1f);
            if (moveVector.x < 0)
                myAvatar.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
            animator.SetBool("move", false);

        if (Physics.Raycast(transform.position, -transform.up, out groundRay, 1))
        {
            jumps = 2;
            jumpCooldown = -1;
        }

        if (jump > 0 && jumpCooldown < 0 && jumps > 0)
        {
            animator.SetTrigger("jump");
            jumps--;
            jumpCooldown = 1;
            myRB.AddForce(transform.up * jump * Time.deltaTime * jumpSpeed);
        }

        jumpCooldown -= Time.deltaTime;
        if (Physics.Raycast(transform.position, Vector3.forward, out interactRay, 1))
        {
            if (interactRay.collider.CompareTag("Ladder"))
            {
                myRB.useGravity = false;
                transform.position += transform.up * moveVector.y * Time.deltaTime * moveSpeed;
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                myRB.velocity = Vector3.zero;
            }
        }
        else
        {
            myRB.useGravity = true;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Water")
        {
            myRB.drag = 1f;
            myRB.useGravity = false;
            myRB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
            mySC.enabled = true;
            this.enabled = false;
        }
    }
}
