using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
