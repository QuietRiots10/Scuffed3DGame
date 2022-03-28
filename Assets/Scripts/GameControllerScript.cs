using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    //Variable Declaration
    public int TargetFrameRate;
    
    //Start
    void Start()
    {
        
    }

    //Update
    void Update()
    {
        Application.targetFrameRate = TargetFrameRate;
    }
}
