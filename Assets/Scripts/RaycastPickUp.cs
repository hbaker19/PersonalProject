using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastPickUp : MonoBehaviour
{
    Camera camera;
    public int ItemHeld = 0;
    public GameObject heldObject;
    public GameObject player;
    float timer = 0;
    float throwPower = 0f;
    Vector3 destination;

    void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        RaycastHit hit;
        if (Input.GetButton("Fire2") && heldObject != null)
        {
            heldObject.layer = 10;

            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit))
            {
                destination = hit.point;
                //hit.point will show the exact place the projectile hits.
            }
            else
            {
                //Still want to shoot
                destination = camera.transform.position + 50 * camera.transform.forward;
            }

            throwPower += Time.deltaTime;
            Debug.Log(throwPower);
            if (throwPower >= 2.0f)
            {
                throwPower = 10;
            }
        }
        if (Input.GetButtonUp("Fire2") && heldObject != null)
        {
            Vector3 velocity = destination - transform.position;
            velocity.Normalize();
            heldObject.GetComponent<Rigidbody>().velocity = velocity * throwPower * 2;

            heldObject.layer = 0;
            //Drop the item.
            heldObject.transform.parent = null;
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            //Log to console.
            Debug.Log("You dropped the " + heldObject);
            //Declare you aren't holding an item.
            heldObject = null;
            //Change ItemHeld to false.
            ItemHeld = 0;
            //Reset timer.
            timer = 0;
            throwPower = 0;
        }
    }

    void FixedUpdate()
    {
        //If the button is pressed, no item is being held, and timer is above 0.3...
        if (Input.GetButtonDown("GrabItem") && ItemHeld == 0 && timer >= 0.3f)
        {
            //Make a raycast.
            RaycastHit hit;
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit))
            {
                Debug.Log(hit.transform.gameObject.name);
                //If raycast hits an item tagged PickUp...
                if(hit.transform.gameObject.tag == "PickUp")
                {
                    //Log to console that an item was found.
                    Debug.Log("I found a pickup");
                    //Declare what the HeldObject is.
                    heldObject = hit.transform.gameObject;
                    //Pick up the item.
                    heldObject.transform.parent = GameObject.FindWithTag("Player").transform;
                    heldObject.GetComponent<Rigidbody>().isKinematic = true;
                    
                    //Change ItemHeld to 1.
                    ItemHeld = 1;
                    //Log to console that the item was grabbed.
                    Debug.Log("You grabbed the " + heldObject);
                    //Reset timer.
                    timer = 0;
                }
            }
        }

        //If the button is pressed while an item is held and the timer is above 0.3...
        if (Input.GetButtonDown("GrabItem") && ItemHeld == 1 && heldObject != null && timer >= 0.3f)
        {
            //Drop the item.
            heldObject.transform.parent = null;
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            //Log to console.
            Debug.Log("You dropped the " + heldObject);
            //Declare you aren't holding an item.
            heldObject = null;
            //Change ItemHeld to false.
            ItemHeld = 0;
            //Reset timer.
            timer = 0;
        }
    }
}
