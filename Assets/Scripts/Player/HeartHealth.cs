using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HeartHealth : MonoBehaviour
{
    private PlayerHandler _playerHandler;

    [Header("Player Statistics")]
    [SerializeField]
    private float curHealth, maxHealth;

    [Header("Heart Slots")]
    public Image[] heartSlots;
    public Sprite[] heartSprite;
    private float healthPerSection;

    private void Start()
    {
        _playerHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHandler>();
        // Calculate the health points per heart section.

        maxHealth = _playerHandler.maxHealth; // Grab the amount of max Health from the player handler.
        healthPerSection = maxHealth / heartSlots.Length;
    }

    private void Update()
    {
        if (curHealth != _playerHandler.curHealth)
        {
            curHealth = _playerHandler.curHealth;
            UpdateHeart();
        }
    }

    void UpdateHeart()
    {
        // Index variable starting at 0 for our slot check.
        // For all the hearts we have:
        for (int i = 0; i < heartSlots.Length; i++)
        {
            // If our health is greater or equal to the slot amount.
            if (curHealth >= (healthPerSection) + healthPerSection * i)
            {
                heartSlots[i].sprite = heartSprite[0]; // Shows player with full hearts
            }

            else
            {
                // You are supposed to be dead here

                heartSlots[i].sprite = heartSprite[1]; // Shows player with no hearts for that section.
            }
        }
    }
}

