using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT SPAWNS BULLETS WHEN ENEMIES SHOOT AND CONTROLS FIRE COOLDOWN

public class EnemyGunScript : MonoBehaviour
{
    //GameObjects
    GameObject CreatedBullet;

    //Variables customized to each gun

    //How far in front of the gun's transform to spawn the bullet
    public float BarrelOffset;
    //How long until you can shoot again
    float FireCooldown;
    //Type of gun
    public string GunType;

    //Prefabs
    GameObject DefaultBulletPrefab;

    IEnumerator Shoot()
    {
        //Determine what type of bullet to spawn
        if (GunType == "Pistol" && FireCooldown <= 0)
        {
            //Instantiate the bullet
            CreatedBullet = Instantiate(DefaultBulletPrefab);
            CreatedBullet.name = "Enemy Pistol Bullet";
            //Set the layer to "Enemy Bullet"
            CreatedBullet.layer = 8;
            //Set the bullet damage dealt to the player by this weapon
            CreatedBullet.GetComponent<BulletScript>().BulletDamage = 13;

            //Move the bullet to the barrel and face it in the correct direction
            CreatedBullet.transform.position = transform.position + transform.forward * BarrelOffset;
            CreatedBullet.transform.LookAt(transform.position + transform.forward * (BarrelOffset + 0.1f));

            //Set the fire cooldown
            FireCooldown = 0.5f;
            yield return null;

        }
        else if (GunType == "AutoRifle" && FireCooldown <= 0)
        { 
            //Instantiate the bullet
            CreatedBullet = Instantiate(DefaultBulletPrefab);
            CreatedBullet.name = "Enemy AutoRifle Bullet";
            //Set the layer to "Enemy Bullet"
            CreatedBullet.layer = 8;
            //Set the bullet damage dealt to the player by this weapon
            CreatedBullet.GetComponent<BulletScript>().BulletDamage = 5;

            //Move the bullet to the barrel and face it in the correct direction
            CreatedBullet.transform.position = transform.position + transform.forward * BarrelOffset;
            CreatedBullet.transform.LookAt(transform.position + transform.forward * (BarrelOffset + 0.1f));

            //Set the fire cooldown
            FireCooldown = 0.25f;
            yield return null;

        }
        yield return null;
    }

    //Start
    void Start()
    {
        //Bullet Prefabs
        DefaultBulletPrefab = Resources.Load("Prefabs/BulletPrefabs/DefaultBulletPrefab") as GameObject;
    }

    //Update
    void Update()
    {
        //Decrement Fire Cooldown
        if (FireCooldown > 0)
        {
            FireCooldown = FireCooldown - Time.deltaTime;
            if (FireCooldown == 0)
            {
                FireCooldown = 0;
            }
        }
    }
}
