using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public class PlayerDataToSave 
{
    //Data...Get from Game
    public string playerName;
    public int level;
    
    public int maxHealth, maxAmmo;
    public int curHealth, curAmmo;
    public float posx, posy;
    
    
    public PlayerDataToSave(PlayerHandler player)
    {
        playerName = player.name;
        level = 0;

        maxHealth = player.maxHealth;
        maxAmmo = player.maxAmmo;
        
        curHealth = player.curHealth;
        curAmmo = player.curAmmo;

        posx = player.transform.position.x;
        posy = player.transform.position.y;

        



    }

}
