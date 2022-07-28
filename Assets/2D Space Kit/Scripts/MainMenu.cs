using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject instrucctionsUI;
    public GameObject mainMenuUI;
    public GameObject creditsUI;

    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void QuitGame ()
    {
        Application.Quit();
    }


    public void Instructions() 
    {
        mainMenuUI.SetActive(false);
        instrucctionsUI.SetActive(true);
    }


    public void InstructionsExit()
    {
        mainMenuUI.SetActive(true);
        instrucctionsUI.SetActive(false);
        creditsUI.SetActive(false);
    }


    public void Credits(){
        mainMenuUI.SetActive(false);
        creditsUI.SetActive(true);
    }


}
