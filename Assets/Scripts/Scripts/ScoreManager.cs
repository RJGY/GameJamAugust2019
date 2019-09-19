
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
   
    public static int score;
    public static int HighScore;
    public Text scoreDisplay;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        // designates the score as a particular object and creates a timer for the auto components
        scoreDisplay = GetComponent<Text>();
        
        //sets screen size to 9:16 ratio
        
    }
    public void Update()
    {
        //displays the change in score
        scoreDisplay.text = "" + _gameManager.score;
        
    }
    
    

}
