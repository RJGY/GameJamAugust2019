﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Member Variables
    public float jumpHeight = 5f; // how high he jumps
    public float climbSpeed = 10f; // how fast he climbs
    public float moveSpeed = 10f; //how fast he moves
    public float curPortal = 10f;
    //private Transform curPortal;
    private CharacterController2D controller;
    private Animator animator;
    //private Interact interactObject;
    /*
    Create an 'interactObject' of Type 'Interact' 
     - Set this variable using the OnTriggerEnter & OnTrigger Exit methods
     - Refer to SunnyLand Project Line 86-91 & Line 96-101
    */

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();

    }

    /*/private void OnDrawGizmos()
    {
        if (curPortal != null)
        {
            float distance = Vector2.Distance(transform.position, curPortal.position);
            if(distance < portalDistance)
            {
                print("Player is within Interactable Distance!");
                curPortal.SendMessage("Interact");
            }
        }
    }
    /*/

    // Update is called once per frame
    void Update()
    {


        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isJumping = Input.GetButtonDown("Jump");

        if (isJumping)
        {
            
            controller.Jump(jumpHeight);


        }

        controller.Climb(vertical * climbSpeed);


        controller.Move(horizontal * moveSpeed);

    }
}
