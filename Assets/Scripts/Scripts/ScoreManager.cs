
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
   
    public static int score;
    public static int HighScore;
    public static Text scoreDisplay;
    public float timer;

    private void Start()
    {
        // designates the score as a particular object and creates a timer for the auto components
        scoreDisplay = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
        InvokeRepeating("Timer", 1, 0.25f);
        //sets screen size to 9:16 ratio
        Screen.SetResolution(800, 1424, true);
    }
    public static void Increase()
    {
        //displays the change in score
        scoreDisplay.text = "Score: " + score;
        
    }
    
    private void Timer()
    {
        // changes the score every second based on the amountPerSecond
        score += Click.amountPerSecond;
        HighScore += Click.amountPerSecond;
        Increase();
    }

}
