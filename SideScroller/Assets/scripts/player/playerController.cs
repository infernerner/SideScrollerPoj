using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    private Rigidbody myRB;
    private CapsuleCollider myCC;
    private Vector3 moveVector = new Vector3(0,0,0);

    private RaycastHit groundRay;
    private RaycastHit interactRay;

    public float moveSpeed;

    private float jump;
    public float jumpSpeed;
    public int jumps = 2;
    public float jumpCooldown;

    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myCC = GetComponent<CapsuleCollider>();
    }

    void OnMove(InputValue value)
    {
        moveVector = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        jump = value.Get<float>();
    }

    void OnInteract()
    {
        if (Physics.Raycast(transform.position, transform.forward, out interactRay, 1))
        {
            if (interactRay.collider.CompareTag("Interactable"))
            {

            }
        }
    }
    public void FixedUpdate()
    {
        transform.position += transform.right * moveVector.x * Time.deltaTime * moveSpeed;

        if (Physics.Raycast(transform.position, -transform.up, out groundRay, 1))
        {
            jumps = 2;
            jumpCooldown = -1;
        }

        if (jumps > 0 && jumpCooldown < 0)
        {
            jumps--;
            jumpCooldown = 1;
            myRB.AddForce(transform.up * jump * Time.deltaTime * jumpSpeed);
        }

        jumpCooldown -= Time.deltaTime;
    }
}
