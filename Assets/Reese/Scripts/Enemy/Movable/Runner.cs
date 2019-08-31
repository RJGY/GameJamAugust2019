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

    // Start is called before the first frame update
    void Start()
    {
        points = waypointParent.GetComponentsInChildren<Transform>();
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
                transform.position = Vector2.MoveTowards(transform.position, currentPlayer.position, speed * Time.deltaTime);
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
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left);
            if (hit.collider != null)
            {
                float distanceOfRaycast = Mathf.Abs(hit.point.x - transform.position.x);
                
                if (distanceOfRaycast < 0.5f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(currentPlayer.position.x, transform.position.y), speed * Time.deltaTime);
                }
            }

            else
            {
                
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(currentPlayer.position.x, transform.position.y), speed * Time.deltaTime);
            }
            
        }

        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right); // move to right
            if (hit.collider != null)
            {
                float distanceOfRaycast = Mathf.Abs(hit.point.x - transform.position.x);
                if (distanceOfRaycast > 0.5f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(currentPlayer.position.x, transform.position.y), speed * Time.deltaTime);
                }
            }

            else
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(currentPlayer.position.x, transform.position.y), speed * Time.deltaTime);
            }
        }

    }

    void SwapStates()
    {
        if (stopRotating)
        {
            Debug.Log("Just rotated");
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
