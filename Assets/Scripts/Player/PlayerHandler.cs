﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHandler : MonoBehaviour
{
    public int maxHealth, maxMana, maxStamina;
    public int curHealth, curMana, curStamina;
    private CharacterController2D controller;
    private UIManager _uiManager;

    void Awake()
    {
        _uiManager = GetComponent<UIManager>();
    }

    void Start()
    {
        maxHealth = 3;
        maxMana = 69;
        maxStamina = 3;
        curHealth = 3;
        curMana = 3;
        curStamina = 3;
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
