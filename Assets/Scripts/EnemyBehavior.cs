using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehavior : MonoBehaviour
{
    //store the nav mesh agent
    NavMeshAgent agent;
    public GameObject player;
    public float chaseDistance = 10;
    private Vector3 home;
    public GameObject tossedObject;
    public float tossedRange = 10;
    public float objectDistance = 10;
    bool hit = false;
    float hitTime = 100;
    public float chaseDuration = 15;

    /*If you want dynamic enemy movement (no home to return to)
    then add a Vector3 startPosition. Set startPos to
    transform.position in Start() and reset startPos to where
    you want the enemy to stop next.*/

    void Start()
    {
        home = transform.position;
        agent = GetComponent<NavMeshAgent>();
}

    // Update is called once per frame
    void Update()
    {
        hitTime += Time.deltaTime;
        /*This makes the enemy chase the player when they're holding an object.
        This can be used later when making the levels of suspicion.
        tossedObject = player.GetComponent<RaycastPickUp>().heldObject;*/

        //This will find the first object tagged Tossed and declare it the tossedObject.
        tossedObject = GameObject.FindGameObjectWithTag("Tossed");
        if (hit)
        {
            //Broken, currently runs every frame.
            player.GetComponent<RaycastPickUp>().Suspicion += 5;
            Debug.Log(player.GetComponent<RaycastPickUp>().Suspicion);

            ChasePlayer();
        }
        else if(tossedObject != null)
        {
            ChaseObject();
        }
        else
        {
            GoHome();
        }
        if(hitTime > chaseDuration)
        {
            hit = false;
        }

        //Calls player's suspicion. If it's too high, enemy will chase.
        if (player.GetComponent<RaycastPickUp>().Suspicion >= 3)
        {
            ChasePlayer();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if object is a tossed object (from player), chase the player.
        if (collision.gameObject.tag == "Tossed")
        {
            hit = true;
            hitTime = 0;
            Debug.Log("You hit the enemy");
        }
    }

    void GoHome()
    {
        agent.destination = home;
        Vector3 homeDirection = home - transform.position;
        if (homeDirection.magnitude < 0.2f)
        {
            home = transform.position;
        }
    }

    void ChaseObject()
    {
        //move the object towards the destination, which in this case
        //is the tossed object
        Vector3 direction = tossedObject.transform.position - transform.position;
        if (direction.magnitude <= tossedRange)
        {
            agent.destination = tossedObject.transform.position;
            Vector3 objectDirection = tossedObject.transform.position - transform.position;
            if (objectDirection.magnitude < 0.2f)
            {
                Debug.Log("Found the object");

                int r = Random.Range(0, 2);

                if (r == 0)
                {
                    GoHome();
                    Debug.Log(r);
                }

                if (r == 1)
                {
                    home = transform.position;
                    Debug.Log(r);
                }
            }
        }
    }

    void ChasePlayer()
    {
        chaseDistance = 30;
        Vector3 direction = player.transform.position - transform.position;
        if (direction.magnitude <= chaseDistance)
        {
            agent.destination = player.transform.position;
            Debug.Log("Chasing player");
        }
        else
        {
            hit = false;

            //Broken, currently runs every frame.
            player.GetComponent<RaycastPickUp>().Suspicion --;
            Debug.Log(player.GetComponent<RaycastPickUp>().Suspicion);
        }
    }
}
