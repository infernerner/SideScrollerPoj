using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class predatorFish : MonoBehaviour
{
    private Rigidbody myRB;
    private RaycastHit myEyes;

    public int FishTier = 1;
    public float swimSpeed = 200;
    public float turnSpeed = 100;
    public Transform turnTransform;
    public GameObject target;
    public Collider[] unfish;
    public List<GameObject> dangerFish;
    public List<GameObject> foodFish;
    public List<GameObject> breedFish;
    private void Start()
    {
        myRB = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        //Debug.Log(Quaternion.Angle(transform.rotation, turnTransform.rotation));
        //Debug.Log(Quaternion.RotateTowards(transform.rotation,turnTransform.rotation,180));
        turnTransform.LookAt(target.transform, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, turnTransform.rotation, 180);
        unfish = Physics.OverlapSphere(transform.position, 10f);
        dangerFish.Clear();
        foodFish.Clear();
        breedFish.Clear();
        foreach (Collider col in unfish)
        {

            if (col.gameObject == gameObject)
            {

            }
            else if (col.gameObject.GetComponent<predatorFish>().FishTier > FishTier)
            {
                dangerFish.Add(col.gameObject);
            }
            else if (col.gameObject.GetComponent<predatorFish>().FishTier < FishTier)
            {
                foodFish.Add(col.gameObject);
            }
            else if (col.gameObject.GetComponent<predatorFish>().FishTier == FishTier)
            {
                breedFish.Add(col.gameObject);
            }

            
        }
    }
}
