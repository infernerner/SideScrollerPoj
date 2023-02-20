using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class intractPickUp : MonoBehaviour
{
    public TextMeshPro interactText;
    public float textTimer;

    void OnInteract()
    {

    }

    private void Update()
    {
        if (textTimer < 0)
            interactText.gameObject.SetActive(false);
    }
}
