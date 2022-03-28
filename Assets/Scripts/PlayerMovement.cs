using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT CONTROLS THE PLAYER'S MOVEMENT

public class PlayerMovement : MonoBehaviour
{
    //Variable Declarations

    //Defines the Player Rigidbody
    Rigidbody PlayerBody;

    //How much to scale up the Input Vector (I.E. how fast the player will move)
    public float PlayerMoveSpeed;
    //How much to scale the Jump Force Vector (I.E. how high the player will jump)
    public float PlayerJumpHeight;
    //Whether the player is grounded
    bool OnGround = false;
    //Whether the player is sprinting
    bool Sprinting = false;
    //Speed Multipliers while sprinting
    public float SprintMult = 1;
    float SprintStrafeMult = 1;

    //Method Declarations
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

            if (OnGround)
            {
                SprintStrafeMult = 1.5f;
            }
            else
            {
                SprintStrafeMult = 1f;
            }

            yield return new WaitForFixedUpdate();
        }

        //When the button is released, return the multipliers to normal
        SprintMult = 1;
        SprintStrafeMult = 1;

        Sprinting = false;
        yield return null;
    }


    //Start
    void Start()
    {
        //Locates the Player Rigidbody
        PlayerBody = transform.GetComponent<Rigidbody>();
    }

    //FixedUpdate is used for Physics Updates (Player Movement)
    void FixedUpdate()
    {
        //Update InputVector, Store Player Input as a unit vector of X and Z inputs
        Vector3 InputVector = transform.forward * Input.GetAxis("ForwardMove") * SprintMult + transform.right * (-1) * Input.GetAxis("HorizontalMove") * SprintStrafeMult;

        //Move
        PlayerBody.MovePosition(transform.position + InputVector * Time.deltaTime * PlayerMoveSpeed);

        //Sprint
        if (OnGround == true && Input.GetButton("Sprint"))
        {
            //Check if the player is already sprinting (to prevent stacking acceleration)
            if (Sprinting == false)
            {
                StartCoroutine(SprintAccelerate());
            }
        }

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
        if (OnGround == true && PlayerBody.velocity.y <= 10)
        {
            PlayerBody.AddForce(Vector3.up * Input.GetAxis("Jump") * PlayerJumpHeight, ForceMode.Impulse);
        }

        //Climbing (TEST)
        if (Physics.Raycast(transform.position, InputVector, 0.5001f, 3) == true)
        {
            //PlayerBody.AddForce(transform.position + Vector3.up * 1000);
            //Debug.Log("Climbing");
        }
    }
}