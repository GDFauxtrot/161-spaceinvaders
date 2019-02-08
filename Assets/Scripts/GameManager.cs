using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int score;
    private int lives;

    [System.NonSerialized]
    public Player player;

    void Awake()
    {
        // Singleton pattern -- publically accessible Instance of GameManager that does not get destroyed
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);

        ResetScoreAndLives();
    }

    public void KillPlayer()
    {
        Debug.Log("Player ded");
    }

    public void ResetScoreAndLives()
    {
        lives = 3;
        score = 0;
    }

    public void AddScore(int newPoints)
    {
        score += newPoints;
    }
}
