using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHandler : MonoBehaviour
{
    public float maxHealth, maxMana, maxStamina;
    public float currentHealth, currentMana, currentStamina;

    private UIManager _uiManager;

    private void Awake()
    {
        _uiManager = GetComponent<UIManager>();
    }

    void Start()
    {
        maxHealth = 100;
        maxMana = 100;
        maxStamina = 100;
        currentHealth = 100;
        currentMana = 100;
        currentStamina = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (_uiManager.healthBar.value != Mathf.Clamp01(currentHealth / maxHealth))
        {
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            _uiManager.healthBar.value = Mathf.Clamp01(currentHealth / maxHealth);
        }
        if (_uiManager.manaBar.value != Mathf.Clamp01(currentMana / maxMana))
        {
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);
            _uiManager.manaBar.value = Mathf.Clamp01(currentMana / maxMana);
        }
        if (_uiManager.staminaBar.value != Mathf.Clamp01(currentStamina / maxStamina))
        {
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            _uiManager.staminaBar.value = Mathf.Clamp01(currentStamina / maxStamina);
        }
    }
}
