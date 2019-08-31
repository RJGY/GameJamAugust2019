using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform currentPlayer = null;
    public Transform gunPosition;
    public GameObject bulletPrefab;
    public bool stopShooting = false;

    // Everything for facing player and shooting
    private float horizontalDistance = 10f;
    private float verticalDistance = 1f;

    // Start is called before the first frame update
    void Start()
    {
        gunPosition = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPlayer != null)
        {
            float checkHorizontalDistance = Mathf.Abs(currentPlayer.position.x - transform.position.x);
            float checkVerticalDistance = Mathf.Abs(currentPlayer.position.y - transform.position.y);

            if ((checkHorizontalDistance < horizontalDistance) && checkVerticalDistance < verticalDistance)
            {
                if (!stopShooting)
                {
                    stopShooting = true;
                    Shoot();
                    Invoke("SwapStates", 0.5f);
                }
            }
        }
    }

    void SwapStates()
    {

        if (stopShooting)
        {
            Debug.Log("Just shot a dude");
            stopShooting = false;
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, gunPosition.position, transform.rotation);
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
