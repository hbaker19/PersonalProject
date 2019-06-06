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
    public int Suspicion = 0;

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
            //Debug.Log(throwPower);
            if (throwPower >= 1.5f)
            {
                throwPower = 10;
            }
        }
        if (Input.GetButtonUp("Fire2") && heldObject != null)
        {
            //If player releases button, change object's tag
            heldObject.tag = "Tossed";
            Vector3 velocity = destination - transform.position;
            velocity.Normalize();
            heldObject.GetComponent<Rigidbody>().velocity = velocity * throwPower * 2;
            heldObject.layer = 0;
            heldObject.transform.parent = null;
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            Debug.Log("You dropped the " + heldObject);
            ItemHeld = 0;
            //Reset timer.
            timer = 0;
            throwPower = 0;
            StartCoroutine(ThrownObject(heldObject));
            heldObject = null;
            Suspicion--;
            Debug.Log(Suspicion);
        }
    }
    private IEnumerator ThrownObject(GameObject thrownObject)
    {
        while (!thrownObject.GetComponent<Rigidbody>().IsSleeping())
        {
            yield return new WaitForSeconds(0.1f);
        }
        thrownObject.tag = "PickUp";
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
                    Debug.Log("I found a pickup");
                    //Declare what the HeldObject is.
                    heldObject = hit.transform.gameObject;
                    //Pick up the item.
                    heldObject.transform.parent = GameObject.FindWithTag("Player").transform;
                    heldObject.GetComponent<Rigidbody>().isKinematic = true;
                    ItemHeld = 1;
                    Debug.Log("You grabbed the " + heldObject);
                    
                    //Reset timer.
                    timer = 0;
                    //Increase suspicion and log total suspicion.
                    Suspicion++;
                    Debug.Log(Suspicion);
                }
            }
        }

        //If the button is pressed while an item is held and the timer is above 0.3...
        if (Input.GetButtonDown("GrabItem") && ItemHeld == 1 && heldObject != null && timer >= 0.3f)
        {
            //Drop the item.
            heldObject.transform.parent = null;
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            Debug.Log("You dropped the " + heldObject);
            heldObject = null;
            ItemHeld = 0;
            timer = 0;
            //Reduce suspicion and log total.
            Suspicion--;
            Debug.Log(Suspicion);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Light")
        {
            player.tag = "PlayerInLight";
            Suspicion++;
            Debug.Log(Suspicion);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        player.tag = "Player";
        Suspicion--;
        Debug.Log(Suspicion);
    }
}
