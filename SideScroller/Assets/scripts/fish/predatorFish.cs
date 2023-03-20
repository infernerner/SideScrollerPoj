using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class predatorFish : MonoBehaviour
{
    private Rigidbody myRB;
    private RaycastHit myEyes;
    private RaycastHit myMouth;
    private RaycastHit water;
    [Header("fight, flight, fuck")]

    public string species;
    public List<string> preyList;
    public List<string> predatorList;

    public GameObject myPlayer;
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
        if (!dead)
        {
            if (FishTier > 0) // makes fish hungry unless its at the bottom of the foodchain
                hungry -= Time.deltaTime;
            else
                hungry += Time.deltaTime;
            if (hungry <= -300) dead = true;
            waterCheck(); // checks if its in or out of water
        }
        if (!dead && inWater)
        {
            fishSight(); // sees all fish within sight that are not blocked by a wall
            sortFish(); // sorts fish by distance
            turnTransform.position = transform.position; // preps turntransform 
            survivalChoice(); // chooses betwen run, eat or breed and then turns the turntransform 
            transform.rotation = Quaternion.RotateTowards(transform.rotation, turnTransform.rotation, 5); // rotates fish
            Vector3 stay2D = new Vector3(transform.forward.x, transform.forward.y, 0); 
            transform.position = new Vector3(transform.position.x, transform.position.y, 0); //puts fish back to z0
            myRB.AddForce(stay2D * swimSpeed);

        }
        else if (dead || !inWater)
        {
            foreach (fishBuoyancy boy in buoyancy)
            {
                boy.dead();
            }
        }
    }
    private void fishSight()
    {
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
                        predatorFish script = col.GetComponent<predatorFish>();
                        foreach (string prey in preyList)
                            if (prey == script.species)
                                foodFish.Add(col.gameObject);
                        foreach (string predator in predatorList)
                            if (predator == script.species)
                                dangerFish.Add(col.gameObject);
                        if (species == script.species)
                            breedFish.Add(col.gameObject);
                    }
                    else if (col.GetComponent<playerStats>() && FishTier > 0)
                        foodFish.Add(col.gameObject);
                }
                else
                    Debug.DrawRay(transform.position, col.transform.position - transform.position, Color.white);
            }
        }
        turnTransform.position = transform.position;
    }
    private void sortFish()
    {
        foodFish.Sort((t2, t1) => Vector3.Distance(gameObject.transform.position, t2.transform.position).CompareTo(Vector3.Distance(gameObject.transform.position, t1.transform.position)));
        dangerFish.Sort((t2, t1) => Vector3.Distance(gameObject.transform.position, t2.transform.position).CompareTo(Vector3.Distance(gameObject.transform.position, t1.transform.position)));
        breedFish.Sort((t2, t1) => Vector3.Distance(gameObject.transform.position, t2.transform.position).CompareTo(Vector3.Distance(gameObject.transform.position, t1.transform.position)));
    }
    private void survivalChoice()
    {
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
                    if (myMouth.collider.GetComponent<predatorFish>()) // eat fish
                    {
                        hungry += 30 * (myMouth.collider.GetComponent<predatorFish>().FishTier + 1);
                        myRB.AddForce(transform.forward * swimSpeed * 10f);
                        if (myMouth.collider.GetComponent<predatorFish>().harpooned)
                        {
                            harpooned = true;
                            myPlayer = myMouth.collider.GetComponent<predatorFish>().myPlayer;
                            myPlayer.GetComponent<SpringJoint>().connectedBody = myRB;
                        }
                        Destroy(foodFish[0].transform.parent.gameObject, 0.1f);
                    }
                    else if (myMouth.collider.GetComponent<playerStats>()) // eat player
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
    }
    private void waterCheck()
    {
        Physics.Raycast(transform.position, Vector3.forward, out water);
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && dead)
        {
            if (harpooned)
            {
                collision.gameObject.GetComponent<weaponController>().readyshot = true;
                collision.gameObject.GetComponent<weaponController>().weapon.SetActive(true); ;
            }
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
