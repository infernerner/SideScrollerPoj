using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class weaponController : MonoBehaviour
{
    public Camera myCamera;
    public GameObject weapon;
    public GameObject instance;
    public bool readyshot = true;

    public Vector3 pointerOffset;
    private Vector3 renderOffset;
    private Vector3 moveVector;
    private PlayerInput input;
    private SpringJoint joint;

    private void Start()
    {
        joint = GetComponent<SpringJoint>();
        input = GetComponent<PlayerInput>();
    }

    private void OnScroll(InputValue value)
    {
        joint.maxDistance += (value.Get<float>() / 240);
        joint.maxDistance = Mathf.Clamp(joint.maxDistance, 0f, 15f);
    }

    void OnFire()
    {
        if (readyshot)
        {
            joint.maxDistance = 8f;
            readyshot = false;
            GameObject bullet = Instantiate(instance, weapon.transform.position + weapon.transform.forward * 2, weapon.transform.rotation);
            bullet.transform.Rotate(90, 0, 0);
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.up * 420f);
            bullet.transform.LookAt(bullet.GetComponent<Rigidbody>().velocity + bullet.transform.position, Vector3.up);
            joint.connectedBody = bullet.GetComponent<Rigidbody>();
            bullet.GetComponent<bulletPhysics>().myPlayer = gameObject;

            weapon.active = false;

        }
    }
    void OnLook(InputValue value)
    {
        pointerOffset = value.Get<Vector2>();
        renderOffset = new Vector3(Display.main.renderingWidth / 2, Display.main.renderingHeight / 2, 0);
        pointerOffset -= renderOffset;
        pointerOffset = pointerOffset / 25;
    }

    void OnMove(InputValue value)
    {
        moveVector = value.Get<Vector2>();
    }

    void Update()
    {
        weapon.transform.LookAt(pointerOffset + transform.position, Vector3.up);
    }
}
