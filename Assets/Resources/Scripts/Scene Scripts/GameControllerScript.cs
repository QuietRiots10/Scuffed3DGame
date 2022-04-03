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
    
    //Methods
    public int GetBaseFOV()
    {
        return BaseFOV;
    }
    public float GetHSense()
    {
        return HSense;
    }
    public float GetVSense()
    {
        return VSense;
    }


    //Update
    void Update()
    {
        Application.targetFrameRate = TargetFrameRate;
    }
}
