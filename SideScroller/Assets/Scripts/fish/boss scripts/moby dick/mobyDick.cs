using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobyDick : MonoBehaviour
{
    public float hitpoints;
    public float speed;
    public float turn;
    public bool dead;
    public GameObject myPlayer;
    public Vector3 vectorOutput;
    public GameObject lookPoint;

    private Rigidbody myRB;

    private float currentMove;
    private bool inWater;

    void Start()
    {
        myRB = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        
        charge();
    }

    private void charge()
    {
        vectorOutput = myPlayer.transform.position - transform.position ;
        transform.LookAt(vectorOutput);
        lookPoint.transform.position = vectorOutput;
    }

    private void move()
    {
        if (inWater && !dead)
        {
            myRB.AddForce(transform.forward * speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            myRB.drag = 5;
            myRB.useGravity = false;
            inWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            myRB.drag = 1;
            myRB.useGravity = true;
            inWater = false;
        }
    }
}
