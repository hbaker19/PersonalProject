using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangePickup : MonoBehaviour
{
    //Attaches to player.
    //Should pick up an object within player's trigger
    //if a key is pressed, object is tagged, and player
    //isn't holding another item.

    public GameObject heldObject;
    public int itemHeld = 0;
    public GameObject player;


    void Start()
    {

    }


    /*void FixedUpdate()
    {
        if (Input.GetButton("GrabItem") && itemHeld == 2)
        {
            heldObject.transform.parent = null;
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            heldObject = null;
            itemHeld = 0;
            Debug.Log("I dropped the item.");
        }

        if (Input.GetButton("GrabItem") && itemHeld == 0)
        {
            heldObject = gameObject;
            heldObject.transform.parent = player.transform;
            heldObject.GetComponent<Rigidbody>().isKinematic = true;
            var handLocation = new Vector3(0, objectDist, 0);
            heldObject.transform.localPosition = handLocation;
            itemHeld = 1;
            Debug.Log("I grabbed the item.");
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp" && Input.GetButtonDown("GrabItem") && itemHeld == 0)
        {
            heldObject = other.gameObject;
            heldObject.transform.parent = GameObject.FindWithTag("Player").transform;
            
        }
    }


}
