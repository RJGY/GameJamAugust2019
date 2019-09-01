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

    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        gunPosition = GetComponent<Transform>();
        anim = GetComponent<Animator>();
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
                    anim.SetBool("IsShooting", true);
                    stopShooting = true;
                    Shoot();
                    Invoke("SwapStates", 0.5f);
                }
            }
            else
            {
                anim.SetBool("IsShooting", false);
            }
        }
    }

    void SwapStates()
    {
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
