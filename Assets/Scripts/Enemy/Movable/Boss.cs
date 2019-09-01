using UnityEngine;

public class Boss : MonoBehaviour
{
    private int moveCounter = 0;

    private int bossHealth = 10;

    public Transform waypointParent;
    private Transform[] points;

    public float killRadius = 3f;

    public int currentWayPoint = 1;

    // See what side the boss is facing.
    public bool facingRight = true;
    public bool facingLeft = false;

    public Transform currentPlayer;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Timer", 1, 1);
        currentPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveCounter % 4 == 0)
        {
            MeleeAttack();
        }

        else if (moveCounter % 4 == 1)
        {
            RangedAttack();
        }

        else if (moveCounter % 4 == 2)
        {
            Screech();
        }

        else
        {
            ChargeAttack();
        }

        if ((currentPlayer.position.x - transform.position.x) < 0) // Move to left
        {
            if (!facingLeft)
            {

            }
        }
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
            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z));;
        }
    }

    void Timer()
    {
        moveCounter++;
    }

    void MeleeAttack()
    {

    }

    void RangedAttack()
    {

    }

    void Screech()
    {

    }

    void ChargeAttack()
    {

    }


}
