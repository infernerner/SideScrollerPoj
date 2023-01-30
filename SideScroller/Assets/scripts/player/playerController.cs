using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    private Rigidbody myRB;
    private CapsuleCollider myCC;
    private Vector3 moveVector = new Vector3(0,0,0);

    public float moveSpeed;
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myCC = GetComponent<CapsuleCollider>();
    }

    void OnMove(InputValue value)
    {
        moveVector = value.Get<Vector2>();
    }

    public void FixedUpdate()
    {
        //transform.position += moveVector * Time.deltaTime * moveSpeed;
        myRB.AddForce(moveVector.x, moveVector.y , 0f);
    }
}
