using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//General Game Settings 
namespace Practice
{
    public class PlayerPrefsSave : MonoBehaviour
    {
        public PlayerHandler player;
        public float x, y;
        public int curScene;

        private void Start()
        {
            player = FindObjectOfType<PlayerHandler>();

            if (!PlayerPrefs.HasKey("Loaded"))
            {
                PlayerPrefs.DeleteAll();
                Load();
                PlayerPrefs.SetInt("Loaded", 0);
            }

        }
        public void Save()
        {
            //Health, Mana, Stamina
            //PLayerPrefs save Float with key CurHealth and the players current hralth value
            PlayerPrefs.SetInt("CurHealth", player.curHealth);
            PlayerPrefs.SetInt("CurAmmo", player.curAmmo);
            
            //Position 
            PlayerPrefs.SetFloat("PosX", player.transform.position.x);
            PlayerPrefs.SetFloat("PosY", player.transform.position.y);

            //Scene Index
            curScene = SceneManager.GetActiveScene().buildIndex;
            
        }

        public void Load()
        {
            GetComponent<MenuButtons>().ChangeScene(curScene);
            //Health, Mana, Stamina
            //PLayers current values is set to PlayerPrefs saved float called CurHealth, else set to MaxHealth
            player.curHealth = PlayerPrefs.GetInt("CurHealth", player.maxHealth);
            player.curAmmo = PlayerPrefs.GetInt("CurAmmo", player.maxAmmo);
           
            //Position 
            x = PlayerPrefs.GetFloat("PosX", 1);
            y = PlayerPrefs.GetFloat("PosY", 1);
            
            player.transform.position = new Vector2(x, y);
            //Build scene
            
            
        }
    }
}
