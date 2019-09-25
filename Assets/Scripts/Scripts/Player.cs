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
        jumpHeight = 5f; // how high he jumps
     climbSpeed = 10f; // how fast he climbs
     moveSpeed = 10f; //how fast he moves
     

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
        if (!controller.IsDead)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            bool isJumping = Input.GetButtonDown("Jump");
            if (Input.GetButtonDown("Fire1"))
            {
                Invoke("Attack", 0.5f);
                SoundManager.Instance.PlaySound("Attack");
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

    void Attack()
    {
        controller.Attack();
    }

}
