using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHandler : MonoBehaviour
{
    public float maxHealth, maxMana, maxStamina;
    public float curHealth, curMana, curStamina;
    private CharacterController2D controller;
    private UIManager _uiManager;

    void Awake()
    {
        _uiManager = GetComponent<UIManager>();
    }

    void Start()
    {
        maxHealth = 3;
        maxMana = 100;
        maxStamina = 100;
        curHealth = 3;
        curMana = 100;
        curStamina = 100;
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (curHealth <= 0)
        {
            controller.Death();
        }
    }
}
