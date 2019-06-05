using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolAndChase : MonoBehaviour
{
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;

    public GameObject player;
    public float chaseDistance = 30;
    public GameObject tossedObject;
    public float tossedRange = 10;
    public float objectDistance = 10;
    bool hit = false;
    float hitTime = 100;
    public float chaseDuration = 15;
    public float playerTooClose = 5;
    public float suspicionTimer = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        //agent.autoBraking = false;

        GotoNextPoint();
    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
        {
            Debug.Log("No points to patrol");
        }

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }

    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();


        hitTime += Time.deltaTime;

        /*This makes the enemy chase the player when they're holding an object.
        This can be used later when making the levels of suspicion.
        tossedObject = player.GetComponent<RaycastPickUp>().heldObject;*/

        //This will find the first object tagged Tossed and declare it the tossedObject.
        tossedObject = GameObject.FindGameObjectWithTag("Tossed");
        if (hit)
        {
            player.GetComponent<RaycastPickUp>().Suspicion += 5;
            hit = false;
        }

        if (player.GetComponent<RaycastPickUp>().Suspicion >= 3)
        {
            Debug.Log(player.GetComponent<RaycastPickUp>().Suspicion);
            ChasePlayer();
        }

        else if (tossedObject != null)
        {
            ChaseObject();
        }
        else
        {
            //GoHome();
        }
        if (hitTime > chaseDuration)
        {
            hit = false;
        }

        //Will add suspicion +1 per second if player is too close to enemy.
        Vector3 direction = player.transform.position - transform.position;
        if (direction.magnitude <= playerTooClose)
        {
            suspicionTimer += Time.deltaTime;
            if (suspicionTimer >= 1f)
            {
                player.GetComponent<RaycastPickUp>().Suspicion += 1;
                Debug.Log("Player is too close. Suspicion " + player.GetComponent<RaycastPickUp>().Suspicion);
                suspicionTimer = 0;
            }
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
                //Never runs. Object falls asleep before enemy reaches it.
                Debug.Log("Found the object");
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
            suspicionTimer += Time.deltaTime;
            if (suspicionTimer >= 1f)
            {
                player.GetComponent<RaycastPickUp>().Suspicion -= 1;
                Debug.Log(player.GetComponent<RaycastPickUp>().Suspicion);
                suspicionTimer = 0;
            }
        }
    }
}
