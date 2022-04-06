using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//THIS SCRIPT CONTROLS THE OPTIONS MENU IN THE PAUSE MENU

public class OptionsMenuScript : MonoBehaviour
{
    //Variables
    GameObject UIObject;
    Canvas PauseMenuCanvas;
    Slider FOVSlider;
    Slider MouseSenseSlider;
    Slider FPSSlider;
    Text FPSSliderText;
    

    //Methods
    public void OptionsClose()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
        PauseMenuCanvas.enabled = true;
    }
    public void FOVChange()
    {
        GameControllerScript.GameController.BaseFOV = Mathf.RoundToInt(FOVSlider.value);
    }
    public void SenseChange()
    {
        GameControllerScript.GameController.HSense = MouseSenseSlider.value;
        GameControllerScript.GameController.VSense = MouseSenseSlider.value;
    }
    public void TargetFPSChange()
    {
        FPSSliderText.fontSize = 40;
        GameControllerScript.GameController.TargetFrameRate = Mathf.RoundToInt(FPSSlider.value);

        if (Mathf.RoundToInt(FPSSlider.value) == 0)
        {
            FPSSliderText.fontSize = 25;
            FPSSliderText.text = "Uncapped";
        }
        else if (Mathf.RoundToInt(FPSSlider.value) == 144)
        {
            FPSSliderText.text = "Vsync";
        }
        else
        {
            FPSSliderText.text = Mathf.RoundToInt(FPSSlider.value) + "";
        }
    }

    //Start
    void Start()
    {
        UIObject = transform.parent.gameObject;
        PauseMenuCanvas = UIObject.transform.GetChild(1).gameObject.GetComponent<Canvas>();

        //Sliders
        FOVSlider = transform.GetChild(2).gameObject.GetComponent<Slider>();
        MouseSenseSlider = transform.GetChild(3).gameObject.GetComponent<Slider>();
        FPSSlider = transform.GetChild(4).gameObject.GetComponent<Slider>();
        FPSSliderText = FPSSlider.transform.GetChild(4).GetChild(0).gameObject.GetComponent<Text>();

        //Set inital values of sliders
        FOVSlider.value = GameControllerScript.GameController.BaseFOV;
        MouseSenseSlider.value = GameControllerScript.GameController.HSense;
        FPSSlider.value = GameControllerScript.GameController.TargetFrameRate;

        //Updates the value of the text label
        TargetFPSChange();
    }
}
