using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

//THIS SCRIPT SPAWNS A BULLET WHEN YOU SHOOT, CONTROLS AMMO AND FIRE RATE,
//AND CONTROLS THROWING YOUR WEAPON

public class WeaponScript : MonoBehaviour
{
    //GameObjects
    GameObject CreatedBullet;
    GameObject CreatedThrownWeapon;
    GameObject FirstPersonCamera;
    AudioSource PlayerAudioSource;
    Text AmmoCounter;

    //Animation
    Animator SwordAnimator;

    //Variables customized to each gun

    //How far in front of the gun's transform to spawn the bullet
    public float BarrelOffset;
    //How long until you can shoot again
    float FireCooldown;
    //Type of gun
    public string GunType;
    public int Ammo;
    public int FullAmmo;

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
                CreatedBullet.transform.LookAt(FirstPersonCamera.transform.position + FirstPersonCamera.transform.forward * (25f));

                //Set the fire cooldown, decrease ammo
                Ammo--;
                PlayerAudioSource.PlayOneShot(Resources.Load("Audio/Gun Fire") as AudioClip);
                AmmoCounter.text = "Ammo: " + Ammo;
                FireCooldown = 0.3f;
                yield return null;
            }
            else
            {
                Debug.Log("Out of Ammo!");
                PlayerAudioSource.PlayOneShot(Resources.Load("Audio/Gun Empty") as AudioClip);
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
                    CreatedBullet.transform.LookAt(FirstPersonCamera.transform.position + FirstPersonCamera.transform.forward * (25f));

                    //Decrease ammo
                    Ammo--;
                    PlayerAudioSource.PlayOneShot(Resources.Load("Audio/Gun Fire") as AudioClip);
                    AmmoCounter.text = "Ammo: " + Ammo;
                }
                else
                {
                    Debug.Log("Out of Ammo!");
                    PlayerAudioSource.PlayOneShot(Resources.Load("Audio/Gun Empty") as AudioClip);
                }

                //Wait for a bit to repeat and fire again
                yield return new WaitForSecondsRealtime(0.15f);
            }

            //Set the fire cooldown
            FireCooldown = 0.2f;
            yield return null;
            
        }
        else if (GunType == "Sword")
        {
            //Play Sword animation
            SwordAnimator.Play("Swing");
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
        AmmoCounter.text = "Ammo: " + 0;

        
        if (GunType == "Sword")
        {
            //Create the thrown weapon object
            CreatedThrownWeapon = Instantiate(Resources.Load("Prefabs/ItemPrefabs/Pickup" + GunType + "Prefab") as GameObject);
            CreatedThrownWeapon.name = "Pickup" + GunType + "Prefab";

            //Set transform and rotation of thrown weapon
            CreatedThrownWeapon.transform.position = transform.position + FirstPersonCamera.transform.forward * BarrelOffset;
            CreatedThrownWeapon.transform.LookAt(transform.position + FirstPersonCamera.transform.forward * (BarrelOffset + 0.1f));

            //Add the force to throw the weapon
            CreatedThrownWeapon.GetComponent<Rigidbody>().AddForce(FirstPersonCamera.transform.forward * 50, ForceMode.VelocityChange);

        }
        else
        {
            //Create the thrown weapon object
            CreatedThrownWeapon = Instantiate(Resources.Load("Prefabs/ItemPrefabs/Thrown" + GunType + "Prefab") as GameObject);
            CreatedThrownWeapon.name = "Thrown " + GunType;

            //Set transform and rotation of thrown weapon
            CreatedThrownWeapon.transform.position = transform.position + FirstPersonCamera.transform.forward * BarrelOffset;
            CreatedThrownWeapon.transform.LookAt(transform.position + FirstPersonCamera.transform.forward * (BarrelOffset + 0.1f));

            //Add the force to throw the weapon
            CreatedThrownWeapon.GetComponent<Rigidbody>().AddForce(FirstPersonCamera.transform.forward * 20, ForceMode.VelocityChange);
        }
    }

    //Start
    void Start()
    {
        FirstPersonCamera = GameObject.FindGameObjectWithTag("MainCamera");
        //Bullet Prefabs
        DefaultBulletPrefab = Resources.Load("Prefabs/BulletPrefabs/DefaultBulletPrefab") as GameObject;
        SwordAnimator = transform.parent.gameObject.GetComponent<Animator>();
    }

    //Refills ammo when you pick up a new gun
    private void OnEnable()
    {
        AmmoCounter = GameObject.Find("AmmoCountDisplay").GetComponent<Text>();
        Ammo = FullAmmo;
        AmmoCounter.text = "Ammo: " + Ammo;

        PlayerAudioSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        PlayerAudioSource.PlayOneShot(Resources.Load("Audio/Gun Cock") as AudioClip);
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
            FireCooldown = FireCooldown - (Time.deltaTime / Time.timeScale);
            Debug.Log(Time.deltaTime);
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
