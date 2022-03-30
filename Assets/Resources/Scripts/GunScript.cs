using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT SPAWNS A BULLET WHEN YOU SHOOT

public class GunScript : MonoBehaviour
{
    //GameObjects
    GameObject CreatedBullet;
    GameObject FirstPersonCamera;

    //Variables customized to each gun
    public float BarrelOffset;
    public string GunType;

    //Prefabs for all the bullet types
    GameObject PistolBulletPrefab;

    //Start
    void Start()
    {
        FirstPersonCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
        PistolBulletPrefab = Resources.Load("Prefabs/PistolBulletPrefab") as GameObject;
    }

    //Update
    void Update()
    {
        if (Input.GetButtonDown("Shoot"))
        {
            //Determine what type of bullet to spawn
            if (GunType == "Pistol")
            {
                CreatedBullet = Instantiate(PistolBulletPrefab);
                CreatedBullet.name = "Pistol Bullet";
            }
            else
            {
                CreatedBullet = null;
                Debug.Log("Define a Gun type before you can shoot!");
            }

            //Move the bullet to the barrel and face it in the correct direction
            CreatedBullet.transform.position = transform.position + FirstPersonCamera.transform.forward * BarrelOffset;
            CreatedBullet.transform.LookAt(transform.position + FirstPersonCamera.transform.forward * (BarrelOffset + 0.1f));
        }
    }
}
