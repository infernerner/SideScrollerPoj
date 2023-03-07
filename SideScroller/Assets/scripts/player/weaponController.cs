using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class weaponController : MonoBehaviour
{
    public Camera myCamera;
    public GameObject weapon;
    private float fireDelay;
    public GameObject projectile;

    public Vector3 pointerOffset;
    private Vector3 renderOffset;
    private Vector3 moveVector;

    void OnFire()
    {
        if (fireDelay < Time.time)
        {
            fireDelay = Time.time + 1;
            GameObject bullet = Instantiate(projectile, weapon.transform.position + weapon.transform.forward * 2, weapon.transform.rotation);
            bullet.transform.Rotate(90, 0, 0);
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.up * 220f);
            bullet.transform.LookAt(bullet.GetComponent<Rigidbody>().velocity + bullet.transform.position, Vector3.up);
            Destroy(bullet, 4);
        }
    }
    void OnLook(InputValue value)
    {
        pointerOffset = value.Get<Vector2>();
        renderOffset = new Vector3(Display.main.renderingWidth / 2, Display.main.renderingHeight / 2, 0);
        pointerOffset -= renderOffset;
        pointerOffset = pointerOffset / 100;
    }

    void OnMove(InputValue value)
    {
        moveVector = value.Get<Vector2>();
    }

    void Update()
    {
        weapon.transform.LookAt(pointerOffset + transform.position - (moveVector), Vector3.up);
    }
}
