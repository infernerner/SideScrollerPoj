using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class intractControllable : MonoBehaviour
{
    protected PlayerInput myPI;

    public GameObject myPlayer;

    public bool intractLock;

    public TextMeshPro interactText;
    public float textTimer;
    void Start()
    {
        myPI = GetComponent<PlayerInput>();
    }

    void OnInteract()
    {
        intractLock = false;
        myPI.enabled = false;
        myPlayer.GetComponent<PlayerInput>().enabled = true;
        myPlayer.GetComponent<Rigidbody>().isKinematic = false;
        myPlayer.transform.parent = null;
    }

    private void Update()
    {
        if (textTimer < 0)
            interactText.gameObject.SetActive(false);
    }
}
