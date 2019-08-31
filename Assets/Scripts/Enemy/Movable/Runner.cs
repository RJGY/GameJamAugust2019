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

    // Everything for facing player and shooting
    private float horizontalDistance = 10f;
    private float verticalDistance = 1f;
    public LayerMask layer;
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
                // Player has been detected.
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(currentPlayer.position.x, transform.position.y), speed * Time.deltaTime);
            }

            else
            {
                // Flip the guard, make it look like he is looking for the alien.
                if (!stopRotating && rotateCount < 4)
                {
                    transform.Rotate(transform.rotation.x, transform.rotation.y + 180, transform.rotation.z);
                    stopRotating = true;
                    Invoke("SwapStates", 1f);
                    rotateCount++;
                }

                else if (rotateCount == 4)
                {
                    Track();
                }
            }
        }
    }

    void Patrol()
    {
        Transform currentPoint = points[currentWayPoint]; // Gets current point.

        float distance = Vector2.Distance(transform.position, currentPoint.position); // Finds distance between itself and the current point.

        transform.position = Vector2.MoveTowards(transform.position, currentPoint.position, speed * Time.deltaTime);

        if (distance < waypointDistance)
        {
            if (currentWayPoint < points.Length - 1)
            {
                currentWayPoint++;
                transform.Rotate(transform.rotation.x, transform.rotation.y + 180, transform.rotation.z);
            }

            else
            {
                // if current waypoints is outside array length
                // reset back to 1
                currentWayPoint = 1;
                transform.Rotate(transform.rotation.x, transform.rotation.y + 180, transform.rotation.z);
                rotateCount = 0;
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
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(currentPlayer.position.x, transform.position.y), speed * Time.deltaTime);
                Debug.Log("NOTHING HERE");
            }
        }

        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right); // Move to right
            if (hit.collider == null)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(currentPlayer.position.x, transform.position.y), speed * Time.deltaTime);
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
        Debug.Log(collision.transform.name);
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
