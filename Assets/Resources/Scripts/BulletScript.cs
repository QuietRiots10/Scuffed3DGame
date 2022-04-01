using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT MOVES THE BULLET AND DETECTS COLLISIONS WITH OBJECTS

public class BulletScript : MonoBehaviour
{
    //Variables
    public float BulletVelocity;
    private int count = 200;

    //Objects
    Rigidbody BulletBody;
    
    //Start
    void Start()
    {
        BulletBody = GetComponent<Rigidbody>();
        BulletBody.AddForce(transform.forward * BulletVelocity, ForceMode.VelocityChange);
    }

    //FixedUpdate will destroy bullets after a certain time
    private void FixedUpdate()
    {
        //Destroy the bullet after a certain amount of time to prevent lag
        if (count <= 0)
        {
            Destroy(gameObject);
        }
        count--;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        Destroy(gameObject);
    }
}
