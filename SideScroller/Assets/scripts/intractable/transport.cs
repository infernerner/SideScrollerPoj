using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class transport : intractControllable
{

    private Vector3 moveVector = new Vector3(0, 0, 0);

    public float moveSpeed = 0;

    void OnMove(InputValue value)
    {
        moveVector = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        transform.position += moveVector * Time.deltaTime * moveSpeed;
    }
}
