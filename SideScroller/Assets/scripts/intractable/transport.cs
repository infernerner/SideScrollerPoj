using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class transport : intractControllable
{

    private Vector3 moveVector = new Vector3(0, 0, 0);

    public float moveSpeed = 0;
    public TextMeshPro interactText;
    public float textTimer;


    //void Start()
    //{
    //    myPI = GetComponent<PlayerInput>();    
    //}

    void OnMove(InputValue value)
    {
        moveVector = value.Get<Vector2>();
    }

    //void OnInteract()
    //{
    //    myPI.enabled = false;
    //    myPlayer.GetComponent<PlayerInput>().enabled = true;
    //    myPlayer.GetComponent<Rigidbody>().isKinematic = false;
    //}

    void FixedUpdate()
    {
        transform.position += moveVector * Time.deltaTime * moveSpeed;
        textTimer -= Time.deltaTime;
        if (textTimer < 0)
            interactText.gameObject.SetActive(false);
    }
}
