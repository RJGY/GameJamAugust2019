using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused;//pause state that everything can see
    private GameObject _pauseMenu;
    void Start()//start of the game set the defaults
    {
        _pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        _pauseMenu.SetActive(false);//hide pause menu
        isPaused = false;//we are not paused
        Time.timeScale = 1;//start time
        Cursor.lockState = CursorLockMode.Locked;//lock cursor to center of screen
        Cursor.visible = false;// hide cursor
    }
    void Update()
    {
        if(Input.GetButtonDown("Cancel"))//press escape 
        {
            TogglePause();//runs toggle pause function
        }
    }
    public void TogglePause()
    {
        if(isPaused)//if we are set to true
        {
            _pauseMenu.SetActive(false);//hide pause menu
            isPaused = false;//we are not paused
            Time.timeScale = 1;//start time
            Cursor.lockState = CursorLockMode.Locked;//lock cursor to center of screen
            Cursor.visible = false;// hide cursor
        }
        else//if we are set to false
        {
            _pauseMenu.SetActive(true);//show pause menu
            isPaused = true;//we are now paused
            Time.timeScale = 0;//stop time
            Cursor.lockState = CursorLockMode.None;//allow cursor movement
            Cursor.visible = true;//show cursor
        }
    }
}
