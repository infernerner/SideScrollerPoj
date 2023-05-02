using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inGameMenuMain : MonoBehaviour
{

    public GameObject mainPanel;
    public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        mainPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Escape))
        {
            if ( paused)
            { // already paused, so resume gameplay
                mainPanel.SetActive(false);
                Time.timeScale = 1;
                paused = false;
            } else { // not paused, so we pause
                mainPanel.SetActive(true);
                Time.timeScale = 0;
                paused = true;
            }
        }
    }
}
