using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastPickUp : MonoBehaviour
{
    Camera camera;
    public int ItemHeld = 0;
    public GameObject heldObject;
    public GameObject player;
    public Rigidbody rb;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        timer += Time.deltaTime;
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
                    rb = heldObject.GetComponent<Rigidbody>();
                    rb.isKinematic = true;

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
