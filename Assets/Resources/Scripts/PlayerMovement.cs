using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT CONTROLS THE PLAYER'S MOVEMENT (RUN, JUMP, SPRINT, TIME STOP, FOV CHANGES)

public class PlayerMovement : MonoBehaviour
{
    //Variable Declarations

    //Defines the Player Rigidbody
    Rigidbody PlayerBody;
    //Defines Player Audio Source
    AudioSource PlayerAudioSource;
    //Defines the GameController Object
    GameObject GameController;
    //Defines Camera Controller Script
    CameraController CameraController;
    //Defines Post Processing Script
    PostProcessingEffectsScript PostProcessingEffectsScript;

    //Stores the Player's inputs
    Vector3 InputVector = new Vector3(0, 0, 0);
    //How much to scale up the Input Vector (I.E. how fast the player will move)
    public float PlayerMoveSpeed;
    //How much to scale the Jump Force Vector (I.E. how high the player will jump)
    public float PlayerJumpHeight;
    //Whether the player is grounded
    bool OnGround = false;
    //Whether the player is sprinting
    bool Sprinting = false;
    //Whether the player is controlling time
    bool TimeStopped = false;
    //How long the player can stop time (0-100 percent)
    public float TimePercent = 100;
    //Speed Multipliers while sprinting
    float SprintMult = 1;
    float SprintStrafeMult = 1;
    //Stores Basic FOV
    int BaseFOV;

    //Temp variables for testing
    

    //Coroutines Declarations
    public float GetTimePercent()
    {
        return TimePercent;
    }
    IEnumerator SprintAccelerate()
    {
        Sprinting = true;

        //While the user holds down the sprint button, increase multipliers
        while (Input.GetButton("Sprint") || OnGround == false)
        {
            //Accelerate
            while (SprintMult < 2)
            {
                SprintMult = SprintMult + 0.05f;
                SprintStrafeMult = SprintStrafeMult + 0.025f;
                yield return new WaitForFixedUpdate();
            }

            SprintMult = 2;

            //Sprint Strafe Mult changes whether you are in the air or not
            if (OnGround && TimeStopped == false)
            {
                SprintStrafeMult = 1.5f;
            }
            else if (TimeStopped == false)
            {
                SprintStrafeMult = 1f;
            }
            else
            {
                SprintStrafeMult = 4f;
            }

            yield return new WaitForFixedUpdate();
        }

        //When the button is released, return the multipliers to normal slowly

        Sprinting = false;
        int count = 25;
        while (count > 0)
        {
            SprintMult -= 0.04f;
            count--;
            yield return new WaitForFixedUpdate();
        }
        
        SprintMult = 1;
        SprintStrafeMult = 1;

        yield return null;
    }
    IEnumerator TimeControl()
    {
        //Time Stop
        TimeStopped = true;
        CameraController.TimeFreeze();
        PostProcessingEffectsScript.CallEffect("TimeEffect", true);
        PlayerAudioSource.PlayOneShot(Resources.Load("Audio/Time Distort") as AudioClip);
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime *= Time.timeScale;
        Physics.gravity = -Vector3.up * 15f;

        while (!Input.GetButtonUp("Za Warudo") && TimePercent > 0)
        {
            TimePercent -= 75f * Time.deltaTime;
            yield return new WaitForSecondsRealtime(0.000001f);
        }

        if (TimePercent <= 0)
        {
            TimePercent = 0f;
        }
        
        CameraController.TimeUnfreeze();
        PostProcessingEffectsScript.CallEffect("TimeEffect", false);
        PlayerAudioSource.PlayOneShot(Resources.Load("Audio/Time Distort Reverse") as AudioClip);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        Physics.gravity = -Vector3.up * 25f;

        yield return new WaitForSecondsRealtime(0.02f);
        TimeStopped = false;
        yield return null;
    }

    //Start
    void Start()
    {
        //Locates the Player Rigidbody
        PlayerBody = transform.GetComponent<Rigidbody>();
        //Locates the Player Audio Source
        PlayerAudioSource = GetComponent<AudioSource>();
        //Locates the GameController Object
        GameController = GameObject.FindGameObjectWithTag("GameController");
        //Locates Camera Controller Script
        CameraController = GetComponent<CameraController>();
        //Locates Time Distorr Script
        PostProcessingEffectsScript = GameObject.Find("Post Processing").GetComponent<PostProcessingEffectsScript>();
    }

    void Update()
    {
        //Get Base FOV
        BaseFOV = GameController.GetComponent<GameControllerScript>().GetBaseFOV();
        
        //Za Warudo (Time Control)
        if (Input.GetButtonDown("Za Warudo"))
        {
            if (!TimeStopped && TimePercent >= 20)
            {
                StartCoroutine(TimeControl());
            }
        }

        //Sprint
        if (OnGround && Input.GetButtonDown("Sprint"))
        {
            //Check if the player is already sprinting (to prevent stacking acceleration)
            if (Sprinting == false)
            {
                StartCoroutine(SprintAccelerate());
            }
        }

        //Regenerate Time Stop Power
        if (!TimeStopped)
        {
            //Slow regeneration under 20%
            if (TimePercent <= 20)
            {
                TimePercent += 7 * Time.deltaTime;
            }
            //Faster regeneration over 20%
            else if (TimePercent <= 100)
            {
                TimePercent += 25 * Time.deltaTime;
            }
        }

        //FOV changes based on speed (Don't apply if time is stopped)
        if (!TimeStopped)
        {
            Camera.main.fieldOfView = BaseFOV * (1 + Math.Abs(Input.GetAxis("ForwardMove") * SprintMult * PlayerMoveSpeed) * 0.01f);
        }
    }

    //FixedUpdate is used for Physics Updates (Player Movement)
    void FixedUpdate()
    {
        //Update InputVector, Store Player Input as a unit vector of X and Z inputs
        InputVector = transform.forward * Input.GetAxis("ForwardMove") * SprintMult + transform.right * (-1) * Input.GetAxis("HorizontalMove") * SprintStrafeMult;

        //Move
        PlayerBody.MovePosition(transform.position + InputVector * Time.deltaTime * PlayerMoveSpeed);

        //Checks if the player is grounded
        if (Physics.BoxCast(transform.position, new Vector3(0.3f, 0.001f, 0.3f), -Vector3.up, out RaycastHit r2, Quaternion.identity, 1.51f, 3))
        {
            if (r2.collider.gameObject.tag == "Ground")
            {
                OnGround = true;
            }
            else
            {
                OnGround = false;
            }
        }
        else
        {
            OnGround = false;
        }

        //Jump
        if (OnGround && PlayerBody.velocity.y <= 10)
        {
            PlayerBody.AddForce(Vector3.up * Input.GetAxis("Jump") * PlayerJumpHeight, ForceMode.Impulse);
        }
    }
}