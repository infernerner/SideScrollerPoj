using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class cameraController : MonoBehaviour
{
    public Vector3 offset;
    public GameObject myPlayer;
    public Vector3 pointerOffset;

    void OnLook(InputValue value)
    {
        pointerOffset = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position,myPlayer.transform.position + offset, Time.deltaTime * 3);
    }
}
