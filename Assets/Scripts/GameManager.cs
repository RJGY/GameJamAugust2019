using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance = null;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public int score = 0; // ScoreKeeping

    public void AddScore(int scoretoAdd)
    {
        // Increase Score Value by incoming score
        score += scoretoAdd;
        // Update UI Here
        UIManager.Instance.UpdateScore(score);
    }
    

    //Reloads the Current Level
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PrevLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void SwitchLevel(int levelID)
    {
        SceneManager.LoadScene(levelID);
    }


}
