﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance = null;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public Text scoreText;

    public Slider healthBar;
    public Slider manaBar;
    public Slider staminaBar;

    public Button respawnButton;
    private void Start()
    {

    }

    public void UpdateScore(int score)
    {
        // Change the score Text to updated value
        scoreText.text = "Diamonds: " + score.ToString();
    }
}
