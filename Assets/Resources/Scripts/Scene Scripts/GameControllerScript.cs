using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT CONTAINS VARIABLES THAT CAN BE CHANGED IN OPTIONS, AND ALLOWS OTHER SCRIPTS TO USE THEM

public class GameControllerScript : MonoBehaviour
{
    //Variable Declaration
    public int TargetFrameRate;
    public int BaseFOV;
    public float HSense;
    public float VSense;

    //Static instance of GameManager that can be accessed, changed, and ran from anywhere
    public static GameControllerScript GameController;

    private void Awake()
    {
        if (GameController == null)
        {
            GameController = this;
            DontDestroyOnLoad(gameObject);
        }

        //Locks Cursor at middle of screen
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Update()
    {
        if (Application.targetFrameRate != TargetFrameRate)
        {
            //Vsync
            if (TargetFrameRate == 144)
            {
                Application.targetFrameRate = Screen.currentResolution.refreshRate;
                Debug.Log(Screen.currentResolution.refreshRate);
            }
            else
            {
                Application.targetFrameRate = TargetFrameRate;
            }
            
        }
    }
}
