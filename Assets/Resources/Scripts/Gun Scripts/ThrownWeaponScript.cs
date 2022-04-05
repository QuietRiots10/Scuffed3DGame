using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT CONTROLS THE DESPAWNING AND DESTRUCTION OF THROWN WEAPONS

public class ThrownWeaponScript : MonoBehaviour
{
    float Despawncount = 5;

    //Fixed Update will despawn the weapon after a certain amount of time
    private void FixedUpdate()
    {
        
        //Destroy the thrown weapon after a certain amount of time to prevent lag
        if (Despawncount <= 0)
        {
            Destroy(gameObject);
        }
        Despawncount = Despawncount - Time.deltaTime;
    }

    //Controls the destruction of the thrown weapon when it hits something
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Thrown Weapon hit: " + collision.gameObject.name);
        //Destroy(gameObject);
    }
}
