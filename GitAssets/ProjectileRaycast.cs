using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRaycast : MonoBehaviour
{
    Camera camera;
    public GameObject prefab;
    public float bulletSpeed = 10.0f;

    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 destination;
            if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit))
            {
                destination = hit.point;
                //hit.point will show the exact place the projectile hits.
            }
            else
            {
                //Still want to shoot
                destination = camera.transform.position + 50 * camera.transform.forward;
            }
            Vector3 velocity = destination - transform.position;
            velocity.Normalize();
            GameObject projectile = Instantiate(prefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().velocity = velocity * bulletSpeed;
        }
    }
}
