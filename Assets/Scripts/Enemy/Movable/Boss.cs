using UnityEngine;

public class Boss : MonoBehaviour
{
    private int moveCounter = 0;

    private int bossHealth = 10;

    public LayerMask layerMask;

    public float killRadius = 3f;

    public int currentWayPoint = 1;

    // See what side the boss is facing.
    public bool facingRight = true;
    public bool facingLeft = false;

    public bool usedSuperMove = false;

    public Transform currentPlayer;

    public GameObject gooBallPrefab;

    public Animator anim;
    private PlayerHandler _playerHandler;
    private Pickup _pickup;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        InvokeRepeating("Timer", 1, 1);
        currentPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveCounter % 2 == 0 && !usedSuperMove)
        {
            anim.SetTrigger("MeleeAttack");
            usedSuperMove = true;
            Invoke("MeleeAttack", 0.5f);
            Invoke("SwapStates", 1f);
        }

        else if (moveCounter % 2 == 1 && !usedSuperMove)
        {
            Invoke("RangedAttack", 0.1f);
            Invoke("RangedAttack", 0.4f);
            Invoke("RangedAttack", 0.7f);
            Invoke("RangedAttack", 1);
            usedSuperMove = true;
            Invoke("SwapStates", 1f);
        }

        if ((currentPlayer.position.x - transform.position.x) < 0) // Move to left
        {
            if (!facingLeft)
            {
                ChangeRotation();
            }
        }

        else if ((currentPlayer.position.x - transform.position.x) > 0) // Move to right
        {
            if (!facingRight)
            {
                ChangeRotation();
            }
        }

        KillPlayer();
    }

    void ChangeRotation()
    {
        if (facingRight) // Turns you left
        {
            facingLeft = true;
            facingRight = false;
            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z));
        }

        else // Turns you right
        {
            facingLeft = false;
            facingRight = true;
            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z)); ;
        }
    }

    void SwapStates()
    {
        usedSuperMove = false;
    }

    void Timer()
    {
        moveCounter++;
    }

    void MeleeAttack()
    {
        if (facingLeft)
        {
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 10f, ~layerMask);
            RaycastHit2D hitTopLeft = Physics2D.Raycast(transform.position, new Vector2(-2, 1), 10f, ~layerMask);
            RaycastHit2D hitBottomLeft = Physics2D.Raycast(transform.position, new Vector2(-2, -1), 10f, ~layerMask);

            if (hitLeft.collider.GetComponent<Player>() != null)
            {
                hitLeft.collider.SendMessage("Death");
            }

            else if (hitTopLeft.collider.GetComponent<Player>() != null)
            {
                hitTopLeft.collider.SendMessage("Death");
            }
            else if (hitBottomLeft.collider.GetComponent<Player>() != null)
            {
                hitBottomLeft.collider.SendMessage("Death");
            }
        }

        if (facingLeft)
        {
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 4f, ~layerMask);
            RaycastHit2D hitTopLeft = Physics2D.Raycast(transform.position, new Vector2(-2, 1), 4f, ~layerMask);
            RaycastHit2D hitBottomLeft = Physics2D.Raycast(transform.position, new Vector2(-2, -1), 4f, ~layerMask);

            if (hitLeft.collider.GetComponent<Player>() != null)
            {
                hitLeft.collider.SendMessage("Death");
            }

            else if (hitTopLeft.collider.GetComponent<Player>() != null)
            {
                hitTopLeft.collider.SendMessage("Death");
            }
            else if (hitBottomLeft.collider.GetComponent<Player>() != null)
            {
                hitBottomLeft.collider.SendMessage("Death");
            }
        }
    }

    void RangedAttack()
    {
        Instantiate(gooBallPrefab, transform.position, transform.rotation);
    }


    void KillPlayer()
    {

        float distance = Vector2.Distance(transform.position, currentPlayer.position);
        if (distance < killRadius && !GameManager.Instance.gameEnded)
        {
            SendMessage("Interact");
            _playerHandler.currentHealth -= 1;
            
            StartCoroutine(_pickup.GotHurt());
        }

    }


}
