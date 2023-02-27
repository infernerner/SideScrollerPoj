using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class predatorFish : MonoBehaviour
{
    private Rigidbody myRB;
    private RaycastHit myEyes;
    private RaycastHit myMouth;
    public float hungry = 0;

    public int FishTier = 1;
    public float swimSpeed = 10;
    public float turnSpeed = 1;
    public float sight = 5;
    public GameObject prefab;
    public Transform turnTransform;
    private Collider[] unfish;
    private List<GameObject> dangerFish;
    private List<GameObject> foodFish;
    private List<GameObject> breedFish;

    private void Start()
    {
        GameObject[] gameObjects = { gameObject };
        myRB = GetComponent<Rigidbody>();
        dangerFish = new List<GameObject>(gameObjects);
        foodFish = new List<GameObject>(gameObjects);
        breedFish = new List<GameObject>(gameObjects);
    }
    void FixedUpdate()
    {
        if (FishTier > 0)
            hungry -= Time.deltaTime;
        else
            hungry += Time.deltaTime;
        if (hungry <= -300) Destroy(gameObject);
        unfish = Physics.OverlapSphere(transform.position, sight);
        dangerFish.Clear();
        foodFish.Clear();
        breedFish.Clear();
        foreach (Collider col in unfish)
        {
            if (Physics.Raycast(transform.position, col.transform.position - transform.position, out myEyes) && col.gameObject.GetComponent<predatorFish>())
            {
                if (myEyes.collider.gameObject == col.gameObject)
                {
                    
                    if (col.gameObject == gameObject) { }
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
                else
                Debug.DrawRay(transform.position, col.transform.position - transform.position, Color.white);
            }

        }
        turnTransform.position = transform.position;
        foodFish.Sort((t2, t1) => Vector3.Distance(gameObject.transform.position, t2.transform.position).CompareTo(Vector3.Distance(gameObject.transform.position, t1.transform.position)));
        dangerFish.Sort((t2, t1) => Vector3.Distance(gameObject.transform.position, t2.transform.position).CompareTo(Vector3.Distance(gameObject.transform.position, t1.transform.position)));
        breedFish.Sort((t2, t1) => Vector3.Distance(gameObject.transform.position, t2.transform.position).CompareTo(Vector3.Distance(gameObject.transform.position, t1.transform.position)));
        if ( dangerFish.Count > 0)
        {
            if (Physics.Raycast(transform.position, dangerFish[0].transform.position - transform.position, out myEyes))
                Debug.DrawRay(transform.position, dangerFish[0].transform.position - transform.position, Color.red);
            turnTransform.LookAt(dangerFish[0].transform, Vector3.up);
            turnTransform.Rotate(Vector3.up * 180);

        }
        else if (breedFish.Count > 0 && hungry > 0)
        {
            if (Physics.Raycast(transform.position, breedFish[0].transform.position - transform.position, out myEyes))
                Debug.DrawRay(transform.position, breedFish[0].transform.position - transform.position, Color.yellow);
            turnTransform.LookAt(breedFish[0].transform, Vector3.up);
            if (Physics.Raycast(transform.position, transform.forward, out myMouth, 1f))
            {
                if (myMouth.collider.gameObject == breedFish[0].gameObject && breedFish[0].GetComponent<predatorFish>().hungry > 0)
                {
                    hungry -= 30 * (FishTier +1);
                    breedFish[0].GetComponent<predatorFish>().hungry -= 30 * (FishTier + 1);
                    Instantiate(prefab);
                }

            }

        }
        else if (foodFish.Count > 0 && hungry < 0)
        {
            if (Physics.Raycast(transform.position, foodFish[0].transform.position - transform.position, out myEyes))
                Debug.DrawRay(transform.position, foodFish[0].transform.position - transform.position, Color.green);
            turnTransform.LookAt(foodFish[0].transform, Vector3.up);
            if (Physics.Raycast(transform.position, transform.forward, out myMouth, 1.5f) )
            {
                if (myMouth.collider.gameObject == foodFish[0].gameObject)
                {
                    hungry += 30 * (foodFish[0].GetComponent<predatorFish>().FishTier +1);
                    myRB.AddForce(transform.forward * swimSpeed * 10f);
                    Destroy(foodFish[0].gameObject,0.1f);
                }
            }
        }
        else if (Physics.Raycast(transform.position, transform.forward, out myEyes,10f))
        {
            Debug.DrawRay(transform.position, transform.forward * myEyes.distance, Color.cyan);
            turnTransform.Rotate(Vector3.left * 2);
            turnTransform.LookAt(turnTransform.forward, Vector3.up);

        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, turnTransform.rotation, 180);
        Vector3 test = new Vector3(transform.forward.x, transform.forward.y, 0);
        Debug.Log(test);
        transform.position = new Vector3(transform.position.x, transform.position.y,0);
        myRB.AddForce(test * swimSpeed);
        
    }
}