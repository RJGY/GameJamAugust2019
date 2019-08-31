using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsSave : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerHandler player;
    private void Start()
    {
        if (!PlayerPrefs.HasKey("Loaded")) // if you dont have the load key
        {
            PlayerPrefs.DeleteAll(); // deletes all the saved data which should nt be there
            Load(); // loads defaults
            PlayerPrefs.SetInt("Loaded", 0); // creates the load key
            BinarySave();
        }

        else
        {
            // binary data
            BinaryLoad();
        }
    }
    public void Save()
    {
        // Health, Mana, Stamina
        PlayerPrefs.SetFloat("currentHealth", player.currentHealth);
        PlayerPrefs.SetFloat("currentMana", player.currentMana);
        PlayerPrefs.SetFloat("currentStamina", player.currentStamina);

        // Rotation
        PlayerPrefs.SetFloat("currentRotationW", player.transform.rotation.w);
        PlayerPrefs.SetFloat("currentRotationX", player.transform.rotation.x);
        PlayerPrefs.SetFloat("currentRotationY", player.transform.rotation.y);
        PlayerPrefs.SetFloat("currentRotationZ", player.transform.rotation.z);

        // Position
        PlayerPrefs.SetFloat("currentPositionX", player.transform.position.x);
        PlayerPrefs.SetFloat("currentPositionY", player.transform.position.y);
        PlayerPrefs.SetFloat("currentPositionZ", player.transform.position.z);
    }

    public void Load()
    {
        // Health, Mana, Stamina
        player.currentHealth = PlayerPrefs.GetFloat("currentHealth", player.maxHealth);
        player.currentMana = PlayerPrefs.GetFloat("currentMana", player.maxMana);
        player.currentStamina = PlayerPrefs.GetFloat("currentStamina", player.maxStamina);

        // Rotation
        player.transform.rotation = new Quaternion(PlayerPrefs.GetFloat("currentRotationX", 0f), PlayerPrefs.GetFloat("currentRotationY", 0f), PlayerPrefs.GetFloat("currentRotationZ", 0f), PlayerPrefs.GetFloat("currentRotationW", 0f));

        // Position
        player.transform.position = new Vector3(PlayerPrefs.GetFloat("currentPositionX", 5f), PlayerPrefs.GetFloat("currentPositionY", 1.5f), PlayerPrefs.GetFloat("currentPositionZ", 5f));
    }

    public void BinarySave()
    {
        PlayerSaveToBinary.SavePlayerData(player);
    }

    public void BinaryLoad()
    {
        PlayerDataToSave data = PlayerSaveToBinary.LoadData(player);

        player.maxHealth = data.maxHealth;
        player.maxMana = data.maxMana;
        player.maxStamina = data.maxStamina;

        player.currentHealth = data.currentHealth;
        player.currentMana = data.currentMana;
        player.currentStamina = data.currentStamina;

        player.transform.position = new Vector3(data.currentPositionX, data.currentPositionY, data.currentPositionZ);
        player.transform.rotation = new Quaternion(data.currentRotationX, data.currentRotationY, data.currentRotationZ, data.currentRotationW);
    }
}
