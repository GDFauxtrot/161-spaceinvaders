using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

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

    public float enemySpeedIncrease, enemyFireRateDecrease;

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
        player.playerDeathEvent.AddListener(KillPlayer);
        ResetScoreAndLives();
    }

    private async void KillPlayer()
    {
        lives--;

        inGameCanvas.SetPlayerLifeCount(lives);

        // Wait for player death anim time before deciding game over
        await Task.Delay(player.deathPauseTimeMs);

        if(lives == 0)
        {
            GameOver();
        }
    }

    public void GameOver()
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

    public int GetScore()
    {
        return score;
    }

    public int GetLives()
    {
        return lives;
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
