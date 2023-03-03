using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class weaponController : MonoBehaviour
{
    public GameObject camera;
    public GameObject weapon;
    private Vector3 aim;
    private Vector3 centerOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnLook(InputValue value)
    {
        aim = value.Get<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        centerOffset = new Vector3(Display.main.renderingWidth / 2, Display.main.renderingHeight / 2, 0f);
        aim -= centerOffset;
        aim = aim / 240;
        weapon.transform.LookAt(new Vector3(aim.x + camera.transform.position.x, aim.y + camera.transform.position.y, 0), Vector3.up);
        Debug.Log(new Vector3(aim.x + camera.transform.position.x, aim.y + camera.transform.position.y, 0));
    }
}
