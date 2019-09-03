using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHandler1 : MonoBehaviour
{
    public float maxHealth, maxMana, maxStamina;
    public float curHealth, curMana, curStamina;

    private UIManager _uiManager;

    void Awake()
    {
        _uiManager = GetComponent<UIManager>();
    }

    void Start()
    {
        maxHealth = 100;
        maxMana = 100;
        maxStamina = 100;
        curHealth = 100;
        curMana = 100;
        curStamina = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
