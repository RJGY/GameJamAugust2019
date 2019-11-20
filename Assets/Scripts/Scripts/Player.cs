using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public AudioClip MusicClip;
    public AudioSource MusicSource;
    //Member Variables
    public float jumpHeight = 5f; // how high he jumps
    public float climbSpeed = 10f; // how fast he climbs
    public float moveSpeed = 10f; //how fast he moves
   
    
    private CharacterController2D controller;
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        //makes the player have a character controller 
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();

      

        jumpHeight = 5f; // how high he jumps

        climbSpeed = 10f; // how fast he climbs
        moveSpeed = 10f; //how fast he moves


    }

    

    // Update is called once per frame
    void Update()
    {
        // when player is not dead the player can use inputs to move their character around and attack
        if (!controller.IsDead)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            bool isJumping = Input.GetButtonDown("Jump");
            if (Input.GetButtonDown("Fire1"))
            {
                Invoke("Attack", 0.5f);
                //SoundManager.Instance.PlaySound("Attack");
            }
            if (isJumping)
            {
                animator.SetBool("IsJumping", true);
                controller.Jump(jumpHeight);
            }

            controller.Climb(vertical * climbSpeed);
            controller.Move(horizontal * moveSpeed);
        }
    }
    //makes your player attack an enemy
   public void Attack()
    {
        controller.Attack();
    }

}
