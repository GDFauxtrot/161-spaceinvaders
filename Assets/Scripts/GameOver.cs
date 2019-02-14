using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverPanel;
    private bool isGameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.gameOverEvent.AddListener(setGameOver);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            gameOverPanel.SetActive(true);
        }
    }

    private void setGameOver()
    {
        isGameOver = true;
    }

    public void GoToMainMenu()
    {
        Destroy(GameObject.FindGameObjectWithTag("GameManager"));
        SceneManager.LoadScene("MainMenuScene");
        

    }

    public void RestartGame()
    {
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
