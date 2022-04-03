using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT ALLOWS THE PLAYER TO PICK UP DROPPED GUNS

public class PickupGunScript : MonoBehaviour
{
    //Objects
    public GameObject GunParent;
    public GameObject FirstPersonCamera;
    public SphereCollider PickupTriggerCollider;

    //Start
    void Start()
    {
        PickupTriggerCollider = gameObject.GetComponent<SphereCollider>();
        FirstPersonCamera = GameObject.FindGameObjectWithTag("MainCamera");
        GunParent = FirstPersonCamera.transform.GetChild(0).gameObject;
    }

    //LateUpdate, to facilitate swapping weapons (it gets dropped first, then picked up in LateUpdate)
    void LateUpdate()
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
                //Name MUST HAVE PICKUP AT THE BEGINNING AND PREFAB AT THE END, AND NO SPACES
                GunParent.transform.Find(hit.collider.gameObject.name.Substring(6, name.Length - 12) + "Gun").gameObject.SetActive(true);
            }
        } 
    }
}
