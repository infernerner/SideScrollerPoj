using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class cannon : intractControllable
{
    public GameObject projectile;
    public Vector3 aim = new Vector3();
    private Vector3 centerOffset = new Vector3();
    private float fireDelay = 0;
    public GameObject cannonObj;

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

    void OnFire()
    {
        if (fireDelay < 0)
        {
            fireDelay = 1;
            GameObject bullet = Instantiate(projectile,cannonObj.transform.position + cannonObj.transform.forward * 4,cannonObj.transform.rotation);
            bullet.transform.Rotate(90, 0, 0);
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.up * 2000f);
            Destroy(bullet, 2);
        }
    }

    private void Update()
    {
        fireDelay -= Time.deltaTime;
    }
}
