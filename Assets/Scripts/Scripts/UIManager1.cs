using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mitchell
{
    public class UIManager1 : MonoBehaviour
    {
        #region Singleton
        public static UIManager1 Instance = null;
        private void Awake()
        {
            Instance = this;
        }
        #endregion

        public Text scoreText;

        public Slider healthBar;
        public Slider manaBar;
        public Slider staminaBar;

        public void UpdateScore(int score)
        {
            // Change the score Text to updated value
            scoreText.text = "Diamonds: " + score.ToString();
        }
    }
}