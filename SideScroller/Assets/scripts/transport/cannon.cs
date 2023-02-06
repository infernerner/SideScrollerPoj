using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class cannon : MonoBehaviour
{

    private PlayerInput myPI;

    public Vector3 aim = new Vector3();
    private Vector3 bim = new Vector3();
    public Vector3 va = new Vector3();

    public GameObject cannonObj;

    public GameObject myPlayer;
    public TextMeshPro interactText;
    public float textTimer;


    void Start()
    {
        myPI = GetComponent<PlayerInput>();
    }

    void OnInteract()
    {
        myPI.enabled = false;
        myPlayer.GetComponent<PlayerInput>().enabled = true;
        myPlayer.GetComponent<Rigidbody>().isKinematic = false;
    }

    void OnLook(InputValue value)
    {
        aim = value.Get<Vector2>();
        bim = new Vector3(Display.main.renderingWidth / 2, Display.main.renderingHeight / 2, 0);
        aim -= bim;
        aim = aim / 180;
        cannonObj.transform.LookAt(aim + transform.position, Vector3.up);
    }

    void FixedUpdate()
    {


        if (textTimer < 0)
            interactText.gameObject.SetActive(false);
    }
}
