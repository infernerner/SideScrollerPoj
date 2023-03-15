using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerStats : MonoBehaviour
{
    public float health = 3f;
    public float invulnerable = 0;
    public float invulnerableTime = 2;
    public List<inventory> bag;

    [System.Serializable]
    public class inventory
    {
        public GameObject item;
        public float amount; 
    }
    private void FixedUpdate()
    {
        if (health < 0f)
            SceneManager.LoadScene(0);
    }
}
