using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void ChangeScene(int sceneIndex)
    {
        //changes the scene to the second and starts the game
        SceneManager.LoadScene(sceneIndex);

    }
    //gets you out of the program
    public void NextLevel()
    {
        GetComponent<MenuButtons>().ChangeScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
