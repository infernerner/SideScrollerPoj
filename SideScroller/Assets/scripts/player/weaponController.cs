using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class weaponController : MonoBehaviour
{
    public Camera myCamera;
    public GameObject weapon;
    private Vector3 aim;
    private float fireDelay;
    public GameObject projectile;

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
    void Update()
    {
        aim = myCamera.ScreenToWorldPoint(Input.mousePosition);
        weapon.transform.LookAt(new Vector3(aim.x, aim.y, 0), Vector3.up);
    }
}
