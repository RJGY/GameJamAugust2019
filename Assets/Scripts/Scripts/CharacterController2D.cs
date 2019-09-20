using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    /* 
     * --- C# TIP ---
     * Use SerializeField to expose private variables
     * Private variables are not accessible through other scripts but will display in the Inspector
    */

    // Member Variables
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping
    [SerializeField] private bool m_StickToSlopes = true;                         // Whether or not a player can stick to slopes
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private LayerMask m_WhatIsEnemy;
    [SerializeField] private LayerMask WhatIsMe;
    //[SerializeField] private LayerMask m_WhatIsLadder;                          // A mask determining what is ladder to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    //[SerializeField] private Transform m_LadderCheck;                           // A position marking where the starting point of ladder ray is.
    [SerializeField] private Transform m_FrontCheck, m_TopCheck, m_AttackCheck, m_HitBox, m_Wrap0, m_Wrap1, m_Wrap2, m_Wrap3;                            // A position makring where to check if the player is not hitting anything
    [SerializeField] private float m_GroundedRadius = .05f;                      // Radius of the overlap circle to determine if grounded
    [SerializeField] private float m_FrontCheckRadius = .05f, m_TopCheckRadius = .05f, m_AttackCheckRadius = 0.05f, m_HitBoxRadius = .05f, m_Wrap0Radius = .05f, m_Wrap1Radius = .05f, m_Wrap2Radius = .05f, m_Wrap3Radius = .05f;                    // Radius of the overlap circle to determine if front is blocked
    [SerializeField] private float m_GroundRayLength = .2f;                     // Length of the ray beneith controller
                                                                                //[SerializeField] private float m_LadderRayLength = .5f;                     // Length of the ray above controller


    private float m_OriginalGravityScale;

    [Header("Events")]
    public UnityEvent OnLandEvent;

    // Public Getters / Setters (Parameters)
    public bool IsGrounded { get; private set; }
    public bool IsClimbing { get; private set; }
    public bool IsFrontBlocked { get; private set; }
    public bool IsTopBlocked { get; private set; }
    public bool Wrap0Free { get; private set; }
    public bool Wrap1Free { get; private set; }
    public bool Wrap2Free { get; private set; }
    public bool Wrap3Free { get; private set; }
    public bool CanHurt { get; private set; }
    public bool IsHurt { get; private set; }
    public bool DoubleJump;
    public bool IsFacingRight = true;
    public bool IsFacingUp = true;
    public float JumpAngle;

    public bool IsDead { get; private set; } = false;
    public Rigidbody2D Rigidbody { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerHandler playerHandler;
    public bool invincible;
    public Animator shield;
    public bool HasParameter(string paramName, Animator animator)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }


    // Internal Methods
    private void Awake()
    {
        playerHandler = GetComponent<PlayerHandler>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        m_OriginalGravityScale = Rigidbody.gravityScale;
        IsHurt = false;
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(m_GroundCheck.position, m_GroundedRadius);
        Gizmos.DrawWireSphere(m_FrontCheck.position, m_FrontCheckRadius);
        Gizmos.DrawWireSphere(m_TopCheck.position, m_TopCheckRadius);
        Gizmos.DrawWireSphere(m_Wrap0.position, m_Wrap0Radius);
        Gizmos.DrawWireSphere(m_Wrap1.position, m_Wrap1Radius);
        Gizmos.DrawWireSphere(m_Wrap2.position, m_Wrap2Radius);
        Gizmos.DrawWireSphere(m_Wrap3.position, m_Wrap3Radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_AttackCheck.position, m_AttackCheckRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(m_HitBox.position, m_HitBoxRadius);
        Ray groundRay = new Ray(transform.position, Vector3.down);
        Gizmos.DrawLine(groundRay.origin, groundRay.origin + groundRay.direction * m_GroundRayLength);



        //Gizmos.color = Color.red;
        //Ray ladderRay = new Ray(m_LadderCheck.position, Vector3.up);
        //Gizmos.DrawLine(ladderRay.origin, ladderRay.origin + ladderRay.direction * m_LadderRayLength);
    }


    private void FixedUpdate()
    {
        if (!IsFacingUp)
        {
            // ... flip the player.
            FlipUp();
        }
        bool wasGrounded = IsGrounded;
        IsGrounded = false;
        Anim.SetBool("IsGrounded", false);
        IsFrontBlocked = false;
        IsTopBlocked = false;
        
        Wrap0Free = false;
        Wrap1Free = false;
        Wrap2Free = false;
        Wrap3Free = false;
        Anim.SetBool("IsRunning", false);
        Anim.SetBool("IsFrontBlocked", false);
        Anim.SetBool("TopSide", false);
        
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, m_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                IsGrounded = true;


                DoubleJump = true;
                Anim.SetBool("IsGrounded", true);
                Anim.SetBool("IsJumping", false);


            }
        }

        colliders = Physics2D.OverlapCircleAll(m_FrontCheck.position, m_FrontCheckRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                IsFrontBlocked = true;
                Anim.SetBool("IsFrontBlocked", true);
                Debug.Log("FB");

            }
        }
        colliders = Physics2D.OverlapCircleAll(m_TopCheck.position, m_TopCheckRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                IsTopBlocked = true;
                Anim.SetBool("TopSide", true);


            }
        }
        colliders = Physics2D.OverlapCircleAll(m_AttackCheck.position, m_AttackCheckRadius, m_WhatIsEnemy);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                CanHurt = true;
            }
        }
        
        
           
            colliders = Physics2D.OverlapCircleAll(m_Wrap0.position, m_Wrap0Radius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    Wrap0Free = true;
                

            }
            }
            colliders = Physics2D.OverlapCircleAll(m_Wrap1.position, m_Wrap1Radius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    Wrap1Free = true;
                

            }
            }
        colliders = Physics2D.OverlapCircleAll(m_Wrap2.position, m_Wrap2Radius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                Wrap2Free = true;
                

            }
        }
        colliders = Physics2D.OverlapCircleAll(m_Wrap3.position, m_Wrap3Radius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                Wrap3Free = true;
                

            }
        }
        
        
        




    }


    public void Attack()
    {
        Anim.SetTrigger("Attack");
        if (IsFacingRight) // ITS ACTUALLY FACING LEFT
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 2f, ~WhatIsMe);
            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<EnemyDeath>() != null)
                {
                    hit.collider.SendMessage("OnDeath");
                }

                else if (hit.collider.GetComponent<Bullet>() != null)
                {
                    hit.collider.SendMessage("Punched");
                }
            }
        }

        else if(!IsFacingRight) // ITS ACTUALLY FACING RIGHT
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 2f, ~WhatIsMe);
            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<EnemyDeath>() != null)
                {
                    hit.collider.GetComponent<EnemyDeath>().SendMessage("OnDeath");
                    Debug.Log("i hit a dude just then its line 251");
                }


            }
        }
        

    }


    public void Flip()
    {
        // Switch the way the player is labelled as facing.
        IsFacingRight = !IsFacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void FlipUp()
    {
        // Switch the way the player is labelled as facing.
        IsFacingUp = !IsFacingUp;

        // Multiply the player's x local scale by -1.
        GetComponent<SpriteRenderer>().flipY = !IsFacingUp;
    }

    // >> Custom methods go here <<

    public void Climb(float offsetY)
    {
        if (HasParameter("ClimbSpeed", Anim))
            Anim.SetFloat("ClimbSpeed", offsetY);

        //RaycastHit2D ladderHit = Physics2D.Raycast(m_LadderCheck.position, Vector2.up, m_LadderRayLength, m_WhatIsLadder);
        if (IsFrontBlocked || IsTopBlocked || Wrap0Free || Wrap3Free || Wrap1Free || Wrap2Free)
        {
            if (offsetY != 0)
            {
                IsClimbing = true;
                Anim.SetBool("IsClimbing", true);

            }
        }
        


        else
        {
            IsClimbing = false;
            Anim.SetBool("IsClimbing", false);

        }

        if (IsClimbing)
        {
            Rigidbody.gravityScale = 0;
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, offsetY);
            if (offsetY > 0 && !IsFacingUp && IsFrontBlocked)
            {
                // ... flip the player.
                FlipUp();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (offsetY < 0 && IsFacingUp)
            {
                // ... flip the player.
                FlipUp();
            }
        }
        else
        {
            Rigidbody.gravityScale = m_OriginalGravityScale;
        }
    }
    public void Jump(float height)
    {

        // If the player should jump...
        if (IsGrounded)
        {
            // Add a vertical force to the player.
            IsGrounded = false;
            Rigidbody.AddForce(new Vector2(0f, height), ForceMode2D.Impulse);
            Anim.SetBool("IsJumping", true);


        }
        else if (DoubleJump && !IsGrounded)
        {
            DoubleJump = false;
            Rigidbody.AddForce(new Vector2(0f, height), ForceMode2D.Impulse);

            Anim.SetTrigger("DoubleJump");


        }
        if (IsFrontBlocked)
        {
            Rigidbody.gravityScale = 0;


            if (IsFacingRight)
            {
                Rigidbody.AddForce(new Vector2(0f, height), ForceMode2D.Impulse);
                Rigidbody.AddForce(new Vector2(-height, 0f), ForceMode2D.Impulse);
            }
            if (!IsFacingRight)
            {
                Rigidbody.AddForce(new Vector2(0f, height), ForceMode2D.Impulse);
                Rigidbody.AddForce(new Vector2(height, 0f), ForceMode2D.Impulse);
            }
            Anim.SetBool("IsJumping", true);
        }
    }

    // Move must be called last!
    public void Move(float offsetX)
    {
        if (offsetX != 0)
        {
            Anim.SetBool("IsRunning", true);
        }

        //only control the player if grounded or airControl is turned on
        if (IsGrounded || m_AirControl)
        {
            if (m_StickToSlopes)
            {
                Ray groundRay = new Ray(transform.position, Vector3.down);
                RaycastHit2D groundHit = Physics2D.Raycast(groundRay.origin, groundRay.direction, m_GroundRayLength, m_WhatIsGround);
                if (groundHit.collider != null)
                {
                    Vector3 slopeDirection = Vector3.Cross(Vector3.up, Vector3.Cross(Vector3.up, groundHit.normal));
                    float slope = Vector3.Dot(Vector3.right * offsetX, slopeDirection);

                    offsetX += offsetX * slope;

                    float angle = Vector2.Angle(Vector3.up, groundHit.normal);
                    if (angle > 0)
                    {
                        Rigidbody.gravityScale = 0;
                        Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 0f);
                    }
                }
            }

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(offsetX, Rigidbody.velocity.y);

            Vector3 velocity = Vector3.zero;
            // And then smoothing it out and applying it to the character
            Rigidbody.velocity = Vector3.SmoothDamp(Rigidbody.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (offsetX > 0 && IsFacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (offsetX < 0 && !IsFacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }

    }

    public void LoseAHeart()
    {
        if (!invincible)
        {
            playerHandler.curHealth -= 1;
            invincible = true;
            shield.SetTrigger("GotHit");
            Invoke("InvincibleSwitch", 2f);
        }
    }

    void InvincibleSwitch()
    {
        invincible = false;
    }

    public void Death()
    {
        Anim.SetTrigger("Dying");
        IsDead = true;
        GameManager.Instance.GameOver();
        // UIManager - Activate red border
        // Place Redborder sprite as a child into the Respawn button.        
        IsHurt = true;

        SoundManager.Instance.PlaySound("PlayerDeath");
    }

    // CTRL + M + O = Folds Code
    // CTRL + M + P = UnFolds Code
}