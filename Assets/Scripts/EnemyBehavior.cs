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
        /*This makes the enemy chase the player when they're holding an object.
        This can be used later when making the levels of suspicion.
        tossedObject = player.GetComponent<RaycastPickUp>().heldObject;*/

        //This will find the first object tagged Tossed and declare it the tossedObject.
        tossedObject = GameObject.FindGameObjectWithTag("Tossed");
        if(tossedObject != null)
        {
            //move the object towards the destination, which in this case
            //is the tossed object
            Vector3 direction = tossedObject.transform.position - transform.position;
            if (direction.magnitude <= tossedRange)
            {
                agent.destination = tossedObject.transform.position;
                Vector3 objectDirection = home - transform.position;
                if (objectDirection.magnitude < 0.3f)
                {
                    Debug.Log("Found the object");
                    GoHome();
                    //Or you can set the enemy to wait. For now we will just go home.
                }
            }
        }
        else
        {
            GoHome();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if object is a tossed object (from player), chase the player.
        if (collision.gameObject.tag == "Tossed")
        {
            chaseDistance = 30;
            Vector3 direction = player.transform.position - transform.position;
            if (direction.magnitude <= chaseDistance)
            {
                //Currently enemy only chases object. Enemy pushes object, making
                //it never fall asleep, making the enemy chase the object with the
                //Tossed tag, which makes the enemy keep pushing it, etc.
                agent.destination = player.transform.position;
            }
        }
    }

    void GoHome()
    {
        agent.destination = home;
        Vector3 homeDirection = home - transform.position;
        if (homeDirection.magnitude < 0.3f)
        {
            home = transform.position;
        }
    }
}
