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

    public GameObject ufoPrefab;

    private int score;
    private int lives;
    float spawnUFOTimer = 10.0f;
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

    void Update()
    {
        spawnUFOTimer -= Time.deltaTime;
        if (spawnUFOTimer <= 0)
        {
            GameObject oneUFO = Instantiate(ufoPrefab);
            UFO ufoScript = oneUFO.GetComponent<UFO>();
            int direction = Random.Range(0, 1);
            if (direction == 0)
            {
                oneUFO.transform.position = new Vector3(8.5f, 5.0f, 0);
                ufoScript.SetDirection(false);
            }
            else
            {
                oneUFO.transform.position = new Vector3(-8.5f, 5.0f, 0);
                ufoScript.SetDirection(true);
            }
            StartTimer();
        }
    }

    public void StartTimer()
    {
        spawnUFOTimer = 10 + Random.Range(0, 20);
    }


    private void KillPlayer()
    {
        lives--;

        inGameCanvas.SetPlayerLifeCount(lives);

        // Wait for player death anim time before deciding game over
        Task.Delay(player.deathPauseTimeMs);

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
