using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT CONTROLS THE DESPAWNING AND DESTRUCTION OF THROWN WEAPONS

public class ThrownWeaponScript : MonoBehaviour
{
    int Despawncount = 1000;

    //Fixed Update will despawn the weapon after a certain amount of time
    private void FixedUpdate()
    {
        
        //Destroy the thrown weapon after a certain amount of time to prevent lag
        if (Despawncount <= 0)
        {
            Destroy(gameObject);
        }
        Despawncount--;
    }

    //Controls the destruction of the thrown weapon when it hits something
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Thrown Weapon hit: " + collision.gameObject.name);
        //Destroy(gameObject);
    }
}
