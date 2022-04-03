using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT SPAWNS A BULLET WHEN YOU SHOOT, CONTROLS AMMO AND FIRE RATE,
//AND CONTROLS THROWING YOUR WEAPON

public class GunScript : MonoBehaviour
{
    //GameObjects
    GameObject CreatedBullet;
    GameObject CreatedThrownWeapon;
    GameObject FirstPersonCamera;

    //Variables customized to each gun

    //How far in front of the gun's transform to spawn the bullet
    public float BarrelOffset;
    //How long until you can shoot again
    float FireCooldown;
    //Type of gun
    public string GunType;
    public int Ammo;

    //Prefabs
    GameObject DefaultBulletPrefab;

    IEnumerator Shoot()
    {
        //Determine what type of bullet to spawn
        if (GunType == "Pistol")
        {
            if (Ammo > 0)
            {
                //Instantiate the bullet
                CreatedBullet = Instantiate(DefaultBulletPrefab);
                CreatedBullet.name = "Pistol Bullet";
                //Set the layer to "Player Bullet"
                CreatedBullet.layer = 7;

                //Move the bullet to the barrel and face it in the correct direction
                CreatedBullet.transform.position = transform.position + FirstPersonCamera.transform.forward * BarrelOffset;
                CreatedBullet.transform.LookAt(transform.position + FirstPersonCamera.transform.forward * (BarrelOffset + 0.1f));

                //Set the fire cooldown, decrease ammo
                Ammo--;
                FireCooldown = 0.3f;
                yield return null;
            }
            else
            {
                Debug.Log("Out of Ammo!");
            }
            
        }
        else if (GunType == "AutoRifle")
        {
            while(Input.GetButton("Shoot"))
            {
                if (Ammo > 0)
                {
                    //Instantiate the bullet
                    CreatedBullet = Instantiate(DefaultBulletPrefab);
                    CreatedBullet.name = "AutoRifle Bullet";
                    //Set the layer to "Player Bullet"
                    CreatedBullet.layer = 7;

                    //Move the bullet to the barrel and face it in the correct direction
                    CreatedBullet.transform.position = transform.position + FirstPersonCamera.transform.forward * BarrelOffset;
                    CreatedBullet.transform.LookAt(transform.position + FirstPersonCamera.transform.forward * (BarrelOffset + 0.1f));

                    //Decrease ammo
                    Ammo--;
                }
                else
                {
                    Debug.Log("Out of Ammo!");
                }

                //Wait for a bit to repeat and fire again
                yield return new WaitForSecondsRealtime(0.15f);
            }

            //Set the fire cooldown
            FireCooldown = 0.2f;
            yield return null;
            
        }
        else
        {
            CreatedBullet = null;
            Debug.Log("Define a Gun type before you can shoot!");
            yield return null;
        }
    }

    void DropWeapon()
    {
        //Deactivate the gun in the inventory
        gameObject.SetActive(false);

        //Create the thrown weapon object
        CreatedThrownWeapon = Instantiate(Resources.Load("Prefabs/ItemPrefabs/Thrown" + GunType + "Prefab") as GameObject);
        CreatedThrownWeapon.name = "Thrown " + GunType;

        //Set transform and rotation of thrown weapon
        CreatedThrownWeapon.transform.position = transform.position + FirstPersonCamera.transform.forward * BarrelOffset;
        CreatedThrownWeapon.transform.LookAt(transform.position + FirstPersonCamera.transform.forward * (BarrelOffset + 0.1f));

        //Add the force to throw the weapon
        CreatedThrownWeapon.GetComponent<Rigidbody>().AddForce(FirstPersonCamera.transform.forward * 20, ForceMode.VelocityChange);
    }

    //Start
    void Start()
    {
        FirstPersonCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
        //Bullet Prefabs
        DefaultBulletPrefab = Resources.Load("Prefabs/BulletPrefabs/DefaultBulletPrefab") as GameObject;
    }

    //Update
    void Update()
    {
        //Fire the gun
        if (Input.GetButtonDown("Shoot"))
        {
            //Checks if firecooldown is too high to shoot
            if (FireCooldown > 0)
            {
                Debug.Log("Gun is on fire cooldown");
            }
            else
            {
                StartCoroutine(Shoot());
            } 
        }

        //Decrement Fire Cooldown
        if (FireCooldown > 0)
        {
            FireCooldown = FireCooldown - Time.deltaTime;
            if (FireCooldown == 0)
            {
                FireCooldown = 0;
            }
        }

        //Drop Weapom
        if (Input.GetButtonDown("Interact"))
        {
            DropWeapon();
        }
    }
}
