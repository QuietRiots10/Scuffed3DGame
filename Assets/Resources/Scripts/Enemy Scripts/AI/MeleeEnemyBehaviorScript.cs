using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//THIS SCRIPT CONTROLS THE AI BEHAVIORS OF THE ENEMY AGENT

public class MeleeEnemyBehaviorScript : MonoBehaviour
{
    //Objects
    NavMeshAgent EnemyAgent;
    public GameObject PlayerObject;
    PlayerMovement PlayerScript;
    Vector3 SpawnPos;
    Vector3 SpawnRot;
    Vector3 LastKnownPos;
    public LayerMask PlayerMask;
    public LayerMask ObstaclesMask;
    GameObject CreatedPickupWeapon;

    //Variables
    public float SightRange = 10;
    public float SightAngle = 90;
    public float AttackRange = 2;

    //States
    public bool PlayerInSightRange;
    public bool PlayerInAttackRange;
    public bool AtSpawnPos;
    public bool AtLastKnownPos;
    public bool ReturningToSpawnPos;

    //HitMarker
    HitMarkerScript HitMarkerScript;

    //Coroutines

    //DROP WEAPON IS BROKEN: NO SWORD ITEM YET
    void DropWeapon()
    {
        //Create the thrown weapon object
        CreatedPickupWeapon = Instantiate(Resources.Load("Prefabs/ItemPrefabs/PickupPistolPrefab") as GameObject);
        CreatedPickupWeapon.name = "PickupPistolPrefab";

        //Set transform and rotation of thrown weapon
        CreatedPickupWeapon.transform.position = transform.position + transform.forward;
        CreatedPickupWeapon.transform.LookAt(transform.position + transform.forward * 0.1f);

        //Add the force to throw the weapon
        CreatedPickupWeapon.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.VelocityChange);
    }
    void AttackPlayer()
    {
        Debug.Log("Attack Player...");
        //ADD AN ANIMATION OF THE SWORD SWINGING, REPLACE IMMEDIATE DAMAGE WITH A COLLISION CHECK AGAINST SWORD
        PlayerScript.TakeDamage(1);
        ReturningToSpawnPos = false;
        EnemyAgent.SetDestination(PlayerObject.transform.position);
        LastKnownPos = PlayerObject.transform.position;
    }
    void ChasePlayer()
    {
        Debug.Log("Chasing Player...");
        ReturningToSpawnPos = false;
        EnemyAgent.SetDestination(PlayerObject.transform.position);
        LastKnownPos = PlayerObject.transform.position;
    }
    void WatchForPlayer()
    {
        //Is the player dead
        if (PlayerScript.Dead)
        {
            ReturningToSpawnPos = false;
            Debug.Log("Haha get rekt noob lol");
            EnemyAgent.speed = 2;
            EnemyAgent.SetDestination(SpawnPos);
        }

        //If the player is not at the spawn position, not at the last known position, or not returning to spawn, chase to the last known location of the player
        //Move to last known position
        else if (!AtSpawnPos && !AtLastKnownPos && !ReturningToSpawnPos)
        {
            ReturningToSpawnPos = false;
            Debug.Log("Lost visual. Moving to last known position...");
            EnemyAgent.speed = 5;

            //Calculate whether the agent can reach the last known position
            NavMeshPath path = new NavMeshPath();
            EnemyAgent.CalculatePath(LastKnownPos, path);
            
            //If the agent can get to the last known position, then go
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                EnemyAgent.path = path;
            }
            //If not, then return to spawn
            else if (path.status == NavMeshPathStatus.PathPartial)
            {
                Debug.Log("Nuts we can't get there. Returning to spawn...");
                EnemyAgent.speed = 2;
                ReturningToSpawnPos = true;
                EnemyAgent.SetDestination(SpawnPos);
            }
        }

        //When the agent reaches the last known position of the player without finding them
        //Return to spawn
        else if (AtLastKnownPos)
        {
            Debug.Log("Target lost. Returning to spawn position...");
            EnemyAgent.speed = 2;
            ReturningToSpawnPos = true;
            EnemyAgent.SetDestination(SpawnPos);
        }

        //If the agent is at it's home position, it will look around for the player. Otherwise, it will return to it's spawn position
        //Look around at spawn
        else if (AtSpawnPos)
        {
            Debug.Log("Watching for Player...");
            if (ReturningToSpawnPos == true)
            {
                StartCoroutine("LookAround");
            }
            ReturningToSpawnPos = false;
        }
    }
    IEnumerator LookAround()
    {
        while (!PlayerInSightRange)
        {
            float count;

            //Rotate 45 degrees right
            count = 0;
            while (count < 1 && !PlayerInSightRange)
            {
                transform.eulerAngles = new Vector3(0, SpawnRot.y + 45 * count, 0);
                count = count + Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(1f);

            //Rotate 45 degrees back to original
            count = 0;
            while (count < 1 && !PlayerInSightRange)
            {
                transform.eulerAngles = new Vector3(0, SpawnRot.y + 45 - 45 * count, 0);
                count = count + Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            //Rotate 45 degrees left
            count = 0;
            while (count < 1 && !PlayerInSightRange)
            {
                transform.eulerAngles = new Vector3(0, SpawnRot.y - 45 * count, 0);
                count = count + Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(1f);

            //Rotate 45 degrees back to original
            count = 0;
            while (count < 1 && !PlayerInSightRange)
            {
                transform.eulerAngles = new Vector3(0, SpawnRot.y - 45 + 45 * count, 0);
                count = count + Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        yield return null;
    }
    bool ViewConeCheck(float Range, float Angle)
    {
        //Creates a list of all colliders withn the viewdistance (on the player layer)
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, Range, PlayerMask);

        //Checks if the list has any elements in it (I.E. If the player is in range)
        if (rangeChecks.Length != 0)
        {
            //Checks the first collider detected, gets the direction to the target
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position + Vector3.up - transform.position).normalized;

            //Checks if the target is within the FOV angle
            if (Vector3.Angle(transform.forward, directionToTarget) < SightAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                //Runs a raycast to check for obstructions in the line of sight
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, ObstaclesMask))
                {
                    return (true);
                }
                else
                {
                    return (false);
                }

            }

            //If the player isn't in the FOV angle, then the agent can't see them
            else
            {
                return (false);
            }

        }
        //If the player isn't in the radius, then the agent can't see them
        else if (PlayerInSightRange)
        {
            return (false);
        }

        else
        {
            return (false);
        }
    }

    //Awake is called whenever the script is instantiated
    private void Awake()
    {
        EnemyAgent = GetComponent<NavMeshAgent>();
        PlayerObject = GameObject.FindGameObjectsWithTag("Player")[0];
        PlayerScript = PlayerObject.GetComponent<PlayerMovement>();
        HitMarkerScript = GameObject.Find("Hitmarker").GetComponent<HitMarkerScript>();
        SpawnPos = transform.position;
        SpawnRot = transform.eulerAngles;
        StartCoroutine("LookAround");
    }

    //Update
    private void Update()
    {
        //Check if the player is in line of sight
        PlayerInSightRange = ViewConeCheck(SightRange, SightAngle);
        PlayerInAttackRange = ViewConeCheck(AttackRange, 360);
        if (PlayerInAttackRange)
        {
            PlayerInSightRange = true;
        }

        //Check if the agent is at it's home position, or the Player's last known position
        AtSpawnPos = new Vector3(transform.position.x, SpawnPos.y, transform.position.z) == SpawnPos;
        AtLastKnownPos = new Vector3(transform.position.x, LastKnownPos.y, transform.position.z) == LastKnownPos;


        //Player is not in attack or sight range
        if ((!PlayerInSightRange && !PlayerInAttackRange) || PlayerScript.Dead)
        {
            WatchForPlayer();
        }

        //Player is in Sight range but not attack range
        else if (PlayerInSightRange && !PlayerInAttackRange)
        {
            EnemyAgent.speed = 7;
            ChasePlayer();
        }

        //Player is in both attack and sight ranges
        else if (PlayerInSightRange && PlayerInAttackRange)
        {
            EnemyAgent.speed = 5;
            AttackPlayer();
        }
    }

    //Detect Collisions with Player Bullets and die
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            Destroy(gameObject);
            //DropWeapon();
            Debug.Log("Killed an enemy!");
            HitMarkerScript.StartCoroutine("StartHitMarker");

        }
        else if (collision.gameObject.layer == 8)
        {
            Destroy(gameObject);
            //DropWeapon();
            Debug.Log("Another enemy killed an enemy!");
        }
    }
}
