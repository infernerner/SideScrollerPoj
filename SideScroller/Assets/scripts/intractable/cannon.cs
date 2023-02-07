using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class cannon : intractControllable
{

    public Vector3 aim = new Vector3();
    private Vector3 centerOffset = new Vector3();

    public GameObject cannonObj;

    public TextMeshPro interactText;
    public float textTimer;

    void OnLook(InputValue value)
    {
        if (intractLock)
        {
            aim = value.Get<Vector2>();
            centerOffset = new Vector3(Display.main.renderingWidth / 2, Display.main.renderingHeight / 2, 0);
            aim -= centerOffset;
            aim = aim / 240;
            cannonObj.transform.LookAt(aim + transform.position, Vector3.up);
        }
    }

    void FixedUpdate()
    {
        if (textTimer < 0)
            interactText.gameObject.SetActive(false);
    }
}
