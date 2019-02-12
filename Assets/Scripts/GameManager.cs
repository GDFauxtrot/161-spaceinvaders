using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public UnityEvent gameOverEvent;

    private int score;
    private int lives;

    [System.NonSerialized]
    public Player player;

    [System.NonSerialized]
    public InGameCanvas inGameCanvas;

    public bool isGamePaused;

    void Awake()
    {
        // Singleton pattern -- publically accessible Instance of GameManager that does not get destroyed
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Player.Instance.PlayerDeath.AddListener(KillPlayer);
        GameObject.FindGameObjectWithTag("EnemyParentObject").GetComponent<UnityEvent>().AddListener(GameOver); //calling this function returns null and prevents ResetScoreAndLives() from running. The Debug log says it isn't referencing an instance of the object and is thus
                                                                                                                    //a null reference
                                                                                                                    //if I try changing EnemyParent to an Instance, it no longer causes a null, but the gameOverEvent still doesn't function properly
        

        ResetScoreAndLives();
    }

    public void KillPlayer()
    {
        Debug.Log("Player ded");
        lives--;
        if(lives == 0)
        {
            GameOver();
        } 
    }

    void GameOver()
    {
        gameOverEvent.Invoke();
    }

    public void ResetScoreAndLives()
    {
        lives = 3;
        score = 0;

        inGameCanvas.SetScoreAndLives(score, lives);
    }

    public void AddPlayerLife()
    {
        lives++;
        inGameCanvas.SetPlayerLifeCount(lives);
    }

    public void AddScore(int newPoints)
    {
        score += newPoints;
        inGameCanvas.SetScoreValue(score);
    }

    public void ToggleGamePause()
    {
        isGamePaused = !isGamePaused;

        Time.timeScale = isGamePaused ? 0 : 1;

        Cursor.visible = isGamePaused;

        inGameCanvas.ShowPauseMenu(isGamePaused);
    }
}
