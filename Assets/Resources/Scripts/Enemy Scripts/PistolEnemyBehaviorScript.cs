using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolEnemyBehaviorScript : MonoBehaviour
{
    //Objects
    GameObject PlayerObject;
    //This is the enemy's first person camera
    GameObject FirstPersonCamera;
    EnemyGunScript EnemyGunScript;

    //Dropped weapon when they die
    GameObject CreatedPickupWeapon;

    //Drops their weapon when they die
    void DropWeapon()
    {
        //Create the thrown weapon object
        CreatedPickupWeapon = Instantiate(Resources.Load("Prefabs/ItemPrefabs/PickupPistolPrefab") as GameObject);
        CreatedPickupWeapon.name = "PickupPistolPrefab";

        //Set transform and rotation of thrown weapon
        CreatedPickupWeapon.transform.position = transform.position + transform.forward;
        CreatedPickupWeapon.transform.LookAt(transform.position + transform.forward *  0.1f);

        //Add the force to throw the weapon
        CreatedPickupWeapon.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.VelocityChange);
    }

    //Start
    void Start()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        FirstPersonCamera = GameObject.FindGameObjectWithTag("MainCamera");
        EnemyGunScript = transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<EnemyGunScript>();
    }

    //Update
    void Update()
    {
        

        EnemyGunScript.StartCoroutine("Shoot");
    }

    //Detect Collisions with Player Bullets and die
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            Destroy(gameObject);
            DropWeapon();
            Debug.Log("Killed an enemy!");
        }
        else if (collision.gameObject.layer == 8)
        {
            Destroy(gameObject);
            DropWeapon();
            Debug.Log("Another enemy killed an enemy!");
        }
    }
}
