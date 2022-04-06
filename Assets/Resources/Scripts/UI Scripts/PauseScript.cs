using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//THIS SCRIPT CONTROLS OPENING THE PAUSE MENU

public class PauseScript : MonoBehaviour
{
    //Variables
    public bool Paused = false;
    GameObject PlayerObj;
    GameObject UIParent;
    Canvas DeathCanvas;
    Canvas OptionsCanvas;
    PlayerMovement PlayerScript;
    PostProcessingEffectsScript EffectsScript;

    //Methods
    public void Pause()
    {
        Paused = true;

        //Pause the time scale of the game
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;

        //Enable the Pause menu canvas
        gameObject.GetComponent<Canvas>().enabled = true;

        //Disable the death screen canvas
        DeathCanvas.enabled = false;

        //Unlock the cursor
        Cursor.lockState = CursorLockMode.None;

        //Disable Player Scripts
        foreach (MonoBehaviour script in PlayerObj.GetComponents<MonoBehaviour>())
        {
            script.enabled = false;
        }
    }
    public void Unpause()
    {
        Paused = false;

        //Pause the time scale of the game
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        //Disable all menu canvases
        gameObject.GetComponent<Canvas>().enabled = false;
        UIParent.transform.GetChild(3).gameObject.GetComponent<Canvas>().enabled = false;

        //Reenable the death screen canvas if the player is dead
        if (PlayerObj.GetComponent<PlayerMovement>().Dead)
        {
            DeathCanvas.enabled = true;
        }
        

        //Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;

        //Enable Player Scripts if they arent dead
        if (!PlayerObj.GetComponent<PlayerMovement>().Dead)
        {
            foreach (MonoBehaviour script in PlayerObj.GetComponents<MonoBehaviour>())
            {
                script.enabled = true;
            }
        }
    }
    public void OptionsOpen()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
        OptionsCanvas.enabled = true;
    }
    public void Restart()
    {
        Unpause();
        EffectsScript.StartCoroutine("DeathEffect", false);
        EffectsScript.StopAllCoroutines();
        PlayerScript.StopAllCoroutines();
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        //Quits in the built game
        Application.Quit();

        //Quits if in the unity editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        
    }

    //Start
    void Start()
    {
        PlayerObj = GameObject.FindGameObjectWithTag("Player");
        gameObject.GetComponent<Canvas>().enabled = false;
        PlayerScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        EffectsScript = GameObject.Find("Post Processing").GetComponent<PostProcessingEffectsScript>();

        UIParent = gameObject.transform.parent.gameObject;
        DeathCanvas = UIParent.transform.GetChild(2).gameObject.GetComponent<Canvas>();
        OptionsCanvas = UIParent.transform.GetChild(3).gameObject.GetComponent<Canvas>();
    }

    //Update
    void Update()
    {
        //Pause
        if (Input.GetButtonDown("Pause") && !Paused)
        {
            Pause();
        }

        //Unpause
        else if (Input.GetButtonDown("Pause") && Paused)
        {
            Unpause(); 
        }

        if (Paused && Time.timeScale != 0)
        {
            //Pause the time scale of the game
            Time.timeScale = 0f;
            Time.fixedDeltaTime = 0f;
        }
    }
}
