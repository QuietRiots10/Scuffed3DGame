using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounterScript : MonoBehaviour
{
    //Variables
    Text FPSCounter;
    
    // Start is called before the first frame update
    void Start()
    {
        FPSCounter = GetComponent<Text>();
        InvokeRepeating("CountFPS", 0.1F, 0.1F);
    }

    // Update is called once per frame
    void CountFPS()
    {
        FPSCounter.text = "FPS: " + (int)(Time.timeScale / Time.deltaTime);
    }
}
