using UnityEngine;
[System.Serializable]

public class PlayerDataToSave
{
    // Data from game
    public string playerName;
    public int level;
    public float maxHealth, maxMana, maxStamina;
    public float currentHealth, currentMana, currentStamina;
    public float currentPositionX, currentPositionY, currentPositionZ;
    public float currentRotationX, currentRotationY, currentRotationZ, currentRotationW;
    

    public PlayerDataToSave(PlayerHandler player)
    {
        playerName = player.name;
        level = 0;

        maxHealth = player.maxHealth;
        maxMana = player.maxMana;
        maxStamina = player.maxStamina;

        currentHealth = player.curHealth;
        currentMana = player.curMana;
        currentStamina = player.curStamina;

        currentPositionX = player.transform.position.x;
        currentPositionY = player.transform.position.y;
        currentPositionZ = player.transform.position.z;

        currentRotationX = player.transform.rotation.x;
        currentRotationW = player.transform.rotation.w;
        currentRotationY = player.transform.rotation.y;
        currentRotationZ = player.transform.rotation.z;

    }
}
