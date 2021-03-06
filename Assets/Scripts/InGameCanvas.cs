﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameCanvas : MonoBehaviour
{
    public TMP_Text scoreValueText;
    public Transform livesGrid;
    public GameObject playerLifePrefab;
    public GameObject pauseMenu;

    void Awake()
    {
        // Will find it, GameManager will always Awake first (see Script Execution Order)
        GameManager.Instance.inGameCanvas = this;
    }

    /// <summary>
    /// Spawns "count" number of player icons to represent lives, deleting old ones.
    /// </summary>
    public void SetPlayerLifeCount(int count)
    {
        // Clear existing children
        foreach (Transform child in livesGrid.transform)
        {
            Destroy(child.gameObject);
        }
        // Add new children count (grid will auto place them yay)
        for (int i = 0; i < count; ++i)
        {
            Instantiate(playerLifePrefab, livesGrid.transform);
        }
    }

    /// <summary>
    /// Sets the score text on screen to the specified value, including commas.
    /// </summary>
    /// <param name="value"></param>
    public void SetScoreValue(int value)
    {
        scoreValueText.text = value.ToString("N0");
    }

    /// <summary>
    /// Calls "SetPlayerLifeCount" and "SetScoreValue" in one go. ez pz
    /// </summary>
    public void SetScoreAndLives(int score, int lives)
    {
        SetScoreValue(score);
        SetPlayerLifeCount(lives);
    }

    /// <summary>
    /// Toggles pause menu visibility.
    /// </summary>
    public void ShowPauseMenu(bool show)
    {
        pauseMenu.SetActive(show);
    }

    public void Button_PauseContinuePressed()
    {
        GameManager.Instance.ToggleGamePause();
    }

    public void Button_PauseQuitPressed()
    {
        Application.Quit(0);
    }
}
