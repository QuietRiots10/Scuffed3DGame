using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT CONTROLS THE MOVEMENTS OF THE PLAYER'S FIRST AND THIRD PERSON CAMERA

public class CameraController : MonoBehaviour
{
    //Defines the Camera Object
    GameObject FirstPersonCamera;
    GameObject ThirdPersonCamera;
    
    //Variable Declaractions
    float HSense;
    float VSense;

    //Start
    void Start()
    {
        //Defines First Person Camera Object
        FirstPersonCamera = GameObject.Find("FirstPersonCamera");
        ThirdPersonCamera = GameObject.Find("ThirdPersonCamera");
    }

    //Update
    void Update()
    {
        //Get HSense and VSense
        HSense = GameControllerScript.GameController.HSense;
        VSense = GameControllerScript.GameController.VSense;

        //Camera Left+Right Rotation (Rotates player)
        transform.Rotate(Vector3.up * (1) * HSense * Input.GetAxis("Mouse X"));

        //Camera Up+Down Rotation (Rotates camera)
        Vector3 angles = new Vector3(FirstPersonCamera.transform.eulerAngles.x + (-1) * VSense * Input.GetAxis("Mouse Y"), transform.eulerAngles.y + (1) * HSense * Input.GetAxis("Mouse X"), 0);

        //Clamp Angle
        if (80f < angles.x && angles.x < 180f)
        {
            angles.x = 80;
        }
        else if (180 < angles.x && angles.x < 280)
        {
            angles.x = 280;
        }

        FirstPersonCamera.transform.eulerAngles = angles;
        
        //Third Person Camera Up+Down Rotation
        ThirdPersonCamera.transform.position = (transform.position + -FirstPersonCamera.transform.forward * 5);
        ThirdPersonCamera.transform.LookAt(transform.position);
        ThirdPersonCamera.transform.position += ThirdPersonCamera.transform.right * 1.8f;
    }
}