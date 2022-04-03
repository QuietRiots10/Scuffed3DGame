using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//THIS SCRIPT UPDATES THE HEALTH BAR UI ELEMENT

public class HealthBarScript : MonoBehaviour
{
    //Variable Declarations
    PlayerMovement PlayerScript;
    Slider HealthBarSlider;
    float Health;

    //Start
    void Start()
    {
        PlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        HealthBarSlider = gameObject.GetComponent<Slider>();
    }

    //Update
    void Update()
    {
        Health = PlayerScript.Health;

        float temp = (HealthBarSlider.value - Health) * Time.deltaTime *  5f;

        HealthBarSlider.value = HealthBarSlider.value - temp;
    }
}

