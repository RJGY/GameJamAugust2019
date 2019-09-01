using UnityEngine;

// TODO - Give the guard the ability to spray the bullets. bullets go everywhere in a 30* cone.
public class Guard : MonoBehaviour
{
    private Transform currentPlayer = null;

    private bool stopRotating = false;

    // Everything for navmesh stuff - patrol
    public Transform waypointParent;
    private Transform[] points;
    public float waypointDistance;
    public float speed = 2f;
    public int currentWayPoint = 1;
    public int rotateCount;
    public float killRadius = 0.5f;
    public bool facingRight = true;
    public bool facingLeft = false;

    // Animation stuff.
    public Animator anim;

    // For gun.
    public Transform gunPosition;
    public GameObject bulletPrefab;
    public bool stopShooting = false;

    // Everything for facing player and shooting
    private float horizontalDistance = 10f;
    private float verticalDistance = 1f;



    void Start()
    {
        gunPosition = GetComponent<Transform>();
        points = waypointParent.GetComponentsInChildren<Transform>();
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        KillPlayer();
        Transform currentPoint = points[currentWayPoint]; // Gets current point.
        if (GameManager.Instance.gameEnded)
        {
            anim.SetBool("IsIdle", true);
        }

        if (currentPlayer == null)
        {
            Patrol();
        }

        else
        {
            // Guard hears you.
            float checkHorizontalDistance = Mathf.Abs(currentPlayer.position.x - transform.position.x);
            float checkVerticalDistance = Mathf.Abs(currentPlayer.position.y - transform.position.y);

            if ((checkHorizontalDistance < horizontalDistance) && checkVerticalDistance < verticalDistance)
            {
                // Player has been detected.
                if ((currentPlayer.position.x - transform.position.x) < 0) // If player is on the right.
                {
                    transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z));
                    if (!stopShooting)
                    {
                        anim.SetTrigger("Attack");
                        stopShooting = true;
                        Shoot();
                        Invoke("SwapStates", 0.5f);
                    }
                }
                else // Player is on the left.
                {
                    transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.x, 180f, transform.rotation.z));
                    if (!stopShooting)
                    {
                        anim.SetTrigger("Attack");
                        stopShooting = true;
                        Shoot();
                        Invoke("SwapStates", 0.5f);
                    }
                }
            }

            else
            {
                // Flip the guard, make it look like he is looking for the alien.
                if (!stopRotating && rotateCount < 4)
                {
                    anim.SetBool("IsWalking", false);
                    transform.Rotate(transform.rotation.x, transform.rotation.y + 180, transform.rotation.z);
                    stopRotating = true;
                    Invoke("SwapStates", 1f);
                    rotateCount++;
                }

                else if (rotateCount == 4)
                {
                    // Guard goes back to patrolling
                    Patrol();
                    anim.SetBool("IsWalking", true);
                }
            }
        }
    }

    void ChangeRotation()
    {
        if (!stopRotating)
        {
            if (facingRight) // Turns you left
            {
                anim.SetBool("IsWalking", false);
                facingLeft = true;
                facingRight = false;
                transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z));
                stopRotating = true;
                Invoke("SwapStates", 1f);
            }

            else // Turns you right
            {
                anim.SetBool("IsWalking", false);
                facingLeft = false;
                facingRight = true;
                transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z));
                stopRotating = true;
                Invoke("SwapStates", 1f);
            }
        }
    }

    void Patrol()
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

        transform.position = Vector2.MoveTowards(transform.position, currentPoint.position, speed * Time.deltaTime);

        if (distance < waypointDistance)
        {
            if (currentWayPoint < points.Length - 1)
            {
                currentWayPoint++;
            }

            else
            {
                currentWayPoint = 1;
                rotateCount = 0;
            }
        }
    }

    void SwapStates()
    {
        if (stopRotating)
        {
            stopRotating = false;
        }

        if (stopShooting)
        {
            stopShooting = false;
        }
    }

    void Shoot()
    {
        if (!GameManager.Instance.gameEnded)
        {
            Instantiate(bulletPrefab, gunPosition.position, transform.rotation);
            SoundManager.Instance.PlaySound("SoldierAttack");
        }
        else
        {
            Debug.Log("This should happen when u are ded");
        }
    }

    void KillPlayer()
    {
        if (currentPlayer != null)
        {
            float distance = Vector2.Distance(transform.position, currentPlayer.position);
            if (distance < killRadius && !GameManager.Instance.gameEnded)
            {
                GameManager.Instance.GameOver();
                SendMessage("Interact");
                Debug.Log("HELP");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentPlayer = collision.transform;
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentPlayer = null;
        }
    }
}
