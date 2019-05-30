using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyNavMove : MonoBehaviour {

    // Use this for initialization
    //store the nav mesh agent
    NavMeshAgent agent;
	public GameObject player;
	public float chaseDistance = 10;
	private Vector3 home;
	void Start () {
		home = transform.position;
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        //move the object towards the destination, which in this case
        //is the player
		Vector3 direction = player.transform.position - transform.position;
        if(direction.magnitude <= chaseDistance){
			agent.destination = player.transform.position;
		}else{
			agent.destination = home;
		}
	}

    /*private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<Rigidbody>().IsSleeping
        if (collision.gameObject.tag == "PickUp" && gameObject.)
        {

        }
    }*/
}
