using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//THIS SCRIPT UPDATES THE TIME BAR UI ELEMENT

public class TimeBarScript : MonoBehaviour
{
    //Variable Declarations
    PlayerMovement PlayerScript;
    Slider TimeBarSlider;
    Image TimeBarFillImage;
    Color TimeBarNormal = new Vector4(0, 231, 255, 255);

    //Start
    void Start()
    {
        PlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        TimeBarSlider = gameObject.GetComponent<Slider>();
        TimeBarFillImage = transform.GetChild(0).GetComponent<Image>();
    }

    //Update
    void Update()
    {
        float TimePercent = PlayerScript.GetTimePercent();
        TimeBarSlider.value = PlayerScript.GetTimePercent();
        
        if (TimePercent <= 20)
        {
            TimeBarFillImage.color = Color.red;
        }
        else
        {
            TimeBarFillImage.color = TimeBarNormal;
        }
    }
}
