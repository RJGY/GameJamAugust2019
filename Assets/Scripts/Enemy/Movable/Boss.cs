using UnityEngine;

public class Boss : MonoBehaviour
{
    private int moveCounter = 0;

    private int bossHealth = 10;

    public LayerMask layerMask;

    private float waypointDistance = 0.5f;
    private float chargeSpeed = 6f;
    public Transform waypointParent;
    private Transform[] points;

    public float killRadius = 3f;

    public int currentWayPoint = 1;

    // See what side the boss is facing.
    public bool facingRight = true;
    public bool facingLeft = false;

    public bool usedSuperMove = false;

    public Transform currentPlayer;

    public GameObject gooBallPrefab;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Timer", 1, 1);
        currentPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveCounter % 4 == 0 && !usedSuperMove)
        {
            anim.SetTrigger("MeleeAttack");
            usedSuperMove = true;
            Invoke("MeleeAttack", 0.5f);
            Invoke("SwapStates", 1f);
        }

        else if (moveCounter % 4 == 1)
        {
            Invoke("RangedAttack", 0.1f);
            Invoke("RangedAttack", 0.4f);
            Invoke("RangedAttack", 0.7f);
            Invoke("RangedAttack", 1);
            usedSuperMove = true;
            Invoke("SwapStates", 1f);
        }

        else if (moveCounter % 4 == 3)
        {
            ChargeAttack();
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
            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z));
        }

        else // Turns you right
        {
            facingLeft = false;
            facingRight = true;
            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z)); ;
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

    void ChargeAttack()
    {
        Transform currentPoint = points[currentWayPoint]; // Gets current point.

        float distance = Vector2.Distance(transform.position, currentPoint.position); // Finds distance between itself and the current point.

        float checkDistance = transform.position.x - currentPoint.position.x;

        if (checkDistance > 0 && !facingLeft)
        {
            Debug.Log("Changed Rotation");
            ChangeRotation();
        }

        else if (!facingRight && checkDistance < 0)
        {
            Debug.Log("Changed Rotation");
            ChangeRotation();
        }

        transform.position = Vector2.MoveTowards(transform.position, currentPoint.position, chargeSpeed * Time.deltaTime);


        if (distance < waypointDistance)
        {
            if (currentWayPoint < points.Length - 1)
            {
                currentWayPoint++;
            }

            else
            {
                Debug.Log("Reset counter.");
                currentWayPoint = 1;
            }
        }
    }

    void KillPlayer()
    {

        float distance = Vector2.Distance(transform.position, currentPlayer.position);
        if (distance < killRadius && !GameManager.Instance.gameEnded)
        {
            SendMessage("Interact");
        }

    }


}
