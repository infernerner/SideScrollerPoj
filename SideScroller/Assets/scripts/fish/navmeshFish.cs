using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navmeshFish : MonoBehaviour
{
    private NavMeshAgent agent;
    private Rigidbody myRB;
    private RaycastHit myEyes;
    private RaycastHit myMouth;
    public float hungry = 0;

    public int FishTier = 1;
    public float swimSpeed = 10;
    public float turnSpeed = 1;
    public float sight = 5;
    public GameObject prefab;
    public Collider[] unfish;
    public List<GameObject> dangerFish;
    public List<GameObject> foodFish;
    public List<GameObject> breedFish;

    private void Start()
    {
        GameObject[] gameObjects = { gameObject };
        agent = GetComponent<NavMeshAgent>();
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
            if (Physics.Raycast(transform.position, col.transform.position - transform.position, out myEyes) && col.gameObject.GetComponent<navmeshFish>())
            {
                if (myEyes.collider.gameObject == col.gameObject)
                {

                    if (col.gameObject == gameObject) { }
                    else if (col.gameObject.GetComponent<navmeshFish>().FishTier > FishTier)
                    {
                        dangerFish.Add(col.gameObject);
                    }
                    else if (col.gameObject.GetComponent<navmeshFish>().FishTier < FishTier)
                    {

                        foodFish.Add(col.gameObject);
                    }
                    else if (col.gameObject.GetComponent<navmeshFish>().FishTier == FishTier)
                    {
                        breedFish.Add(col.gameObject);
                    }
                }
                else
                    Debug.DrawRay(transform.position, col.transform.position - transform.position, Color.white);
            }

        }
        foodFish.Sort((t2, t1) => Vector3.Distance(gameObject.transform.position, t2.transform.position).CompareTo(Vector3.Distance(gameObject.transform.position, t1.transform.position)));
        dangerFish.Sort((t2, t1) => Vector3.Distance(gameObject.transform.position, t2.transform.position).CompareTo(Vector3.Distance(gameObject.transform.position, t1.transform.position)));
        breedFish.Sort((t2, t1) => Vector3.Distance(gameObject.transform.position, t2.transform.position).CompareTo(Vector3.Distance(gameObject.transform.position, t1.transform.position)));
        if (dangerFish.Count > 0)
        {
            if (Physics.Raycast(transform.position, dangerFish[0].transform.position - transform.position, out myEyes))
                Debug.DrawRay(transform.position, dangerFish[0].transform.position - transform.position, Color.red);
            agent.destination = dangerFish[0].transform.forward * 5 + transform.position;

        }
        else if (breedFish.Count > 0 && hungry > 0)
        {
            if (Physics.Raycast(transform.position, breedFish[0].transform.position - transform.position, out myEyes))
                Debug.DrawRay(transform.position, breedFish[0].transform.position - transform.position, Color.yellow);
            agent.destination = breedFish[0].transform.position;
            if (Physics.Raycast(transform.position, transform.forward, out myMouth, 1f))
            {
                if (myMouth.collider.gameObject == breedFish[0].gameObject && breedFish[0].GetComponent<navmeshFish>().hungry > 0)
                {
                    hungry -= 30 * (FishTier + 1);
                    breedFish[0].GetComponent<navmeshFish>().hungry -= 30 * (FishTier + 1);
                    Instantiate(prefab);
                }

            }

        }
        else if (foodFish.Count > 0 && hungry < 0)
        {
            if (Physics.Raycast(transform.position, foodFish[0].transform.position - transform.position, out myEyes))
                Debug.DrawRay(transform.position, foodFish[0].transform.position - transform.position, Color.green);
            agent.destination = foodFish[0].transform.position;
            if (Physics.Raycast(transform.position, transform.forward, out myMouth, 1.5f))
            {
                if (myMouth.collider.gameObject == foodFish[0].gameObject)
                {
                    hungry += 30 * (foodFish[0].GetComponent<navmeshFish>().FishTier + 1);
                    myRB.AddForce(transform.forward * swimSpeed * 10f);
                    Destroy(foodFish[0].transform.parent.gameObject, 0.1f);
                }
            }
        }

    }
}
