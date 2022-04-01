using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT ALLOWS THE PLAYER TO PICK UP DROPPED GUNS

public class PickupGunScript : MonoBehaviour
{
    //Objects
    Rigidbody PlayerBody;
    GameObject GunParent;
    GameObject FirstPersonCamera;
    SphereCollider PickupTriggerCollider;

    //Start
    void Start()
    {
        PlayerBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        PickupTriggerCollider = gameObject.GetComponent<SphereCollider>();
        FirstPersonCamera = GameObject.FindGameObjectWithTag("MainCamera");
        GunParent = FirstPersonCamera.transform.GetChild(0).gameObject;
    }

    //Update
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            //Shoot a raycast to check whether the player is looking at the item
            Physics.Raycast(FirstPersonCamera.transform.position, FirstPersonCamera.transform.forward, out RaycastHit hit, 5, 3);
            if (hit.collider == PickupTriggerCollider)
            {
                //Destroy the dropped gun
                Destroy(gameObject);

                //Find the gun in the player's inventory and activate it (I.E. make the player hold it)
                GunParent.transform.Find(hit.collider.gameObject.name.Substring(6, name.Length - 12) + "Gun").gameObject.SetActive(true);
            }
        } 
    }
}
