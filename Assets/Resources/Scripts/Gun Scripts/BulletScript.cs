using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT MOVES THE BULLET AND DETECTS COLLISIONS WITH OBJECTS

public class BulletScript : MonoBehaviour
{
    //Variables
    public float BulletVelocity;
    private int count = 200;
    public float BulletDamage;

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

    //Determines what the bullet collided with, damages the player if applicable
    private void OnCollisionEnter(Collision collision)
    {
        //Damage the player, if the collider hit belongs to the player
        if(collision.gameObject.tag == "Player" && BulletDamage != 0)
        {
            collision.gameObject.SendMessageUpwards("TakeDamage", BulletDamage);
        }
        else if (collision.gameObject.tag == "Player" && BulletDamage == 0)
        {
            Debug.Log("Weird shit happened, BulletDamage isn't defined. Did you hit yourself with your own bullet??");
        }

        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        Destroy(gameObject);
    }
}
