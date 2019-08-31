using UnityEngine;

public class Runner : MonoBehaviour
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

    // Everything for facing player and shooting
    private float horizontalDistance = 10f;
    private float verticalDistance = 1f;
    public LayerMask layer;
    public bool facingLeft = false;
    public bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        points = waypointParent.GetComponentsInChildren<Transform>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x - 1, transform.position.y, transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        Transform currentPoint = points[currentWayPoint]; // Gets current point.

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

                if (!stopRotating)
                {
                    KillPlayer();
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(currentPlayer.position.x, transform.position.y), speed * Time.deltaTime);
                }
            }

            else
            {
                // Flip the guard, make it look like he is looking for the alien.
                if (!stopRotating && rotateCount < 4)
                {
                    Rotate();
                }

                else if (rotateCount == 4)
                {
                    Track();
                }
            }
        }
    }

    void Rotate()
    {
        Debug.Log("JUST ROTATED IN Rotate()");
        transform.Rotate(transform.rotation.x, transform.rotation.y + 180, transform.rotation.z);
        stopRotating = true;
        Invoke("SwapStates", 1f);
        rotateCount++;
    }

    void ChangeRotation()
    {
        if (!stopRotating)
        {
            if (facingRight) // Turns you left
            {
                facingLeft = true;
                facingRight = false;
                transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z));
                stopRotating = true;
                Invoke("SwapStates", 1f);
            }

            else // Turns you right
            {
                facingLeft = false;
                facingRight = true;
                transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z));
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
                Debug.Log("Add to counter");
                currentWayPoint++;
            }

            else
            {
                // if current waypoints is outside array length
                // reset back to 1
                Debug.Log("Reset counter.");
                currentWayPoint = 1;
            }
        }
    }

    void Track()
    {
        if ((currentPlayer.position.x - transform.position.x) < 0) // Move to left
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 1f, ~layer);
            if (hit.collider == null)
            {
                if(!facingLeft)
                {
                    ChangeRotation();
                }
                    
                if (!stopRotating)
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(currentPlayer.position.x, transform.position.y), speed * Time.deltaTime);
                }
            }
        }

        else if ((currentPlayer.position.x - transform.position.x) > 0) // Move to right
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 1f, ~layer);
            if (hit.collider == null)
            {
                if (!facingRight)
                {
                    ChangeRotation();
                }

                if (!stopRotating)
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(currentPlayer.position.x, transform.position.y), speed * Time.deltaTime);
                }
            }
        }

        else
        {
            rotateCount = 0;
            // THIS COULD ALSO CAUSE A BUG TOO
        }

    }

    void KillPlayer()
    {
        if (currentPlayer != null)
        {
            float distance = Vector2.Distance(transform.position, currentPlayer.position);
            if (distance < killRadius)
            {
                // Works but kills them over and over.
                SendMessage("Interact");
            }
        }
    }

    void SwapStates()
    {
        if (stopRotating)
        {
            stopRotating = false;
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
