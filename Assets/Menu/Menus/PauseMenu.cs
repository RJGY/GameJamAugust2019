using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused;//pause state that everything can see
    private GameObject _pauseMenu;
    private GameObject _pauseButton;
    private GameObject _restartButton;
    
    private GameObject _pauseInitial;
    public Text _pauseTitle;
    // Reference
    private CharacterController2D controller;
    void Awake()//start of the game set the defaults
    {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();

        _pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        _pauseButton = GameObject.FindGameObjectWithTag("PauseButoon");
        _restartButton = GameObject.FindGameObjectWithTag("Respawn");

        _pauseTitle = GameObject.FindGameObjectWithTag("PauseTitle").GetComponent<Text>();

        _restartButton.SetActive(false);
        _pauseMenu.SetActive(true); // show pause menu
        isPaused = true;//we are  paused
        Time.timeScale = 0;//start time
    }
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !controller.IsDead) //press escape 
        {
            TogglePause();//runs toggle pause function
        }
        if (controller.IsDead)
        {
            DeathScreen();
        }
    }
    public void Start()
    {
        TogglePause();
        


    }
    public void TogglePause()
    {
        if(isPaused)//if we are set to true
        {

            _pauseMenu.SetActive(false);//hide pause menu
            isPaused = false;//we are not paused
            Time.timeScale = 1;//start time
            
        }
        else//if we are set to false
        {
            _pauseMenu.SetActive(true);//show pause menu
            _pauseTitle.text = "Paused";
            isPaused = true;//we are now paused
            Time.timeScale = 0;//stop time
            
        }
    }
    public void DeathScreen()
    {
        
            _pauseMenu.SetActive(true);//show pause menu
        _pauseTitle.text = "You are dead";
        isPaused = true;//we are now paused
            Time.timeScale = 0;//stop time
            _pauseButton.SetActive(false);
            _restartButton.SetActive(true);
            //_pauseTitle.text = "Game Over";
        
    }
}
