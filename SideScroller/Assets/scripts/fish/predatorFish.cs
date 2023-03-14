using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class predatorFish : MonoBehaviour
{
    private Rigidbody myRB;
    private RaycastHit myEyes;
    private RaycastHit myMouth;
    private RaycastHit water;
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
    private fishBuoyancy[] buoyancy;
    public bool dead = false;
    public bool inWater = true;
    public bool harpooned = false;

    private void Start()
    {
        GameObject[] gameObjects = { gameObject };
        myRB = GetComponent<Rigidbody>();
        dangerFish = new List<GameObject>(gameObjects);
        foodFish = new List<GameObject>(gameObjects);
        breedFish = new List<GameObject>(gameObjects);
        buoyancy = transform.GetComponentsInChildren<fishBuoyancy>();
    }
    void FixedUpdate()
    {
        if (!dead && inWater)
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
                if (Physics.Raycast(transform.position, col.transform.position - transform.position, out myEyes) && col.gameObject.GetComponent<predatorFish>() || col.gameObject.GetComponent<landController>())
                {
                    if (myEyes.collider.gameObject == col.gameObject)
                    {
                        if (col.gameObject == gameObject) { }
                        if (col.GetComponent<predatorFish>())
                        {
                            if (col.gameObject.GetComponent<predatorFish>().FishTier > FishTier)
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
                        else if (col.GetComponent<playerStats>() && FishTier > 0)
                            foodFish.Add(col.gameObject);
                    }
                    else
                        Debug.DrawRay(transform.position, col.transform.position - transform.position, Color.white);
                }
            }
            turnTransform.position = transform.position;
            foodFish.Sort((t2, t1) => Vector3.Distance(gameObject.transform.position, t2.transform.position).CompareTo(Vector3.Distance(gameObject.transform.position, t1.transform.position)));
            dangerFish.Sort((t2, t1) => Vector3.Distance(gameObject.transform.position, t2.transform.position).CompareTo(Vector3.Distance(gameObject.transform.position, t1.transform.position)));
            breedFish.Sort((t2, t1) => Vector3.Distance(gameObject.transform.position, t2.transform.position).CompareTo(Vector3.Distance(gameObject.transform.position, t1.transform.position)));
            if (dangerFish.Count > 0)
            {
                if (Physics.Raycast(transform.position, dangerFish[0].transform.position - transform.position, out myEyes))
                    Debug.DrawRay(transform.position, dangerFish[0].transform.position - transform.position, Color.red);
                turnTransform.LookAt(dangerFish[0].transform, Vector3.up);
                turnTransform.Rotate(Vector3.up, 180f);

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
                        hungry -= 30 * (FishTier + 1);
                        myMouth.collider.GetComponent<predatorFish>().hungry -= 30 * (FishTier + 1);
                        Instantiate(prefab);
                    }

                }

            }
            else if (foodFish.Count > 0 && hungry < 0)
            {
                if (Physics.Raycast(transform.position, foodFish[0].transform.position - transform.position, out myEyes))
                    Debug.DrawRay(transform.position, foodFish[0].transform.position - transform.position, Color.green);
                turnTransform.LookAt(foodFish[0].transform, Vector3.up);
                if (Physics.Raycast(transform.position, transform.forward, out myMouth, 1.5f))
                {
                    if (myMouth.collider.gameObject == foodFish[0].gameObject)
                    {
                        if (myMouth.collider.GetComponent<predatorFish>())
                        {
                            hungry += 30 * (myMouth.collider.GetComponent<predatorFish>().FishTier + 1);
                            myRB.AddForce(transform.forward * swimSpeed * 10f);
                            Destroy(foodFish[0].transform.parent.gameObject, 0.1f);
                        }
                        else if (myMouth.collider.GetComponent<playerStats>())
                        {
                            var stats = myMouth.collider.GetComponent<playerStats>();
                            if (stats.invulnerable < Time.time)
                            {
                                stats.invulnerable = Time.time + stats.invulnerableTime;
                                stats.health--;
                                myMouth.collider.GetComponent<Rigidbody>().AddForce(-80 * (transform.position - myMouth.collider.transform.position));
                            }

                        }
                    }
                }
            }
            else if (Physics.Raycast(transform.position, transform.forward, out myEyes, 10f))
            {
                Debug.DrawRay(transform.position, transform.forward * myEyes.distance, Color.cyan);
                turnTransform.Rotate(Vector3.left, 2);
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, turnTransform.rotation, 180);
            Vector3 stay2D = new Vector3(transform.forward.x, transform.forward.y, 0);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            myRB.AddForce(stay2D * swimSpeed);

        }
        else if (dead || !inWater)
        {
            foreach (fishBuoyancy boy in buoyancy)
            {
                boy.dead();
            }
        }
        Physics.Raycast(transform.position, Vector3.forward, out water);
        if (!dead)
        {
            if (water.collider == null)
            {
                inWater = false;
                turnTransform.forward *= -1;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, turnTransform.rotation, 180);
                Vector3 stay2D = new Vector3(transform.forward.x, transform.forward.y, 0);
                myRB.AddForce(stay2D * swimSpeed);
                transform.position += stay2D;

            }
            else if (water.collider.tag != "Water")
            {
                inWater = false;
                turnTransform.forward *= -1;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, turnTransform.rotation, 180);
                Vector3 stay2D = new Vector3(transform.forward.x, transform.forward.y, 0);
                myRB.AddForce(stay2D * swimSpeed);
                transform.position += stay2D;
            }
            else if (water.collider.tag == "Water")
            {
                inWater = true;
            }
        } 
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && dead)
        {
            if (harpooned)
            {
                collision.gameObject.GetComponent<weaponController>().readyshot = true;
                collision.gameObject.GetComponent<weaponController>().weapon.active = true;
            }
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
