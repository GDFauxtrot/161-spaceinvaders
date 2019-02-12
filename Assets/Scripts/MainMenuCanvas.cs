using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCanvas : MonoBehaviour
{
    
    public GameObject creditsPanel;

    public void Button_StartPressed()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Button_CreditsPressed()
    {
        creditsPanel.SetActive(true);
    }

    public void Button_CreditsBackPressed()
    {
        creditsPanel.SetActive(false);
    }

    public void Button_QuitPressed()
    {
        Application.Quit(0);
    }
}
