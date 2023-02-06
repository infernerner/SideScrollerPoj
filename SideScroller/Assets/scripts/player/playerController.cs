using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    private Rigidbody myRB;
    private CapsuleCollider myCC;
    private PlayerInput myPI;
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
        myPI = GetComponent<PlayerInput>();
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
                transform.parent = interactRay.collider.transform.parent;
                myPI.enabled = false;
                myRB.isKinematic = true;
                var change = interactRay.collider.transform.parent.gameObject.GetComponent<PlayerInput>();
                change.enabled = true;
                if (change.GetComponent<transport>())
                {
                    change.GetComponent<transport>().myPlayer = gameObject;
                }
                if (change.GetComponent<cannon>())
                {
                    change.GetComponent<cannon>().myPlayer = gameObject;
                }
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

        if (jump > 0 && jumpCooldown < 0 && jumps > 0)
        {
            jumps--;
            jumpCooldown = 1;
            myRB.AddForce(transform.up * jump * Time.deltaTime * jumpSpeed);
        }

        jumpCooldown -= Time.deltaTime;
        if (Physics.Raycast(transform.position, transform.forward, out interactRay, 1))
        {
            if (interactRay.collider.CompareTag("Interactable"))
            {
                if  (interactRay.collider.transform.parent.gameObject.GetComponent<transport>())
                {
                    interactRay.collider.transform.parent.gameObject.GetComponent<transport>().interactText.gameObject.SetActive(true);
                    interactRay.collider.transform.parent.gameObject.GetComponent<transport>().textTimer = 1;
                }
                if (interactRay.collider.transform.parent.gameObject.GetComponent<cannon>())
                {
                    interactRay.collider.transform.parent.gameObject.GetComponent<cannon>().interactText.gameObject.SetActive(true);
                    interactRay.collider.transform.parent.gameObject.GetComponent<cannon>().textTimer = 1;
                }

            }
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
}
