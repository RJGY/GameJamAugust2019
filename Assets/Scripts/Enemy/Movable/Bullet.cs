using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Change this value later to the player script.
    private Transform currentPlayer;

    private float bulletSpeed = 4f;

    private bool movingToRight = false;
    private bool movingToLeft = false;

    private Vector3 startingPoint;
    private PlayerHandler _playerHandler;
    private Pickup _pickup;
    // Start is called before the first frame update
    void Start()
    {
        startingPoint = transform.position;
        // Change this later on to the player script.
        currentPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if ((currentPlayer.position.x - transform.position.x) < 0) // If player is on the left.
        {
            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0f));
            movingToRight = true;
            
        }

        else
        {
            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180f));
            movingToLeft = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Change expiration to a horizontal distance.
        if (Vector2.Distance(startingPoint, transform.position) > 10f)
        {
            Destroy(gameObject);
        }


        if(movingToRight)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x - 9, transform.position.y), bulletSpeed * Time.deltaTime);
        }
        else if (movingToLeft)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + 9, transform.position.y), bulletSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        _playerHandler.currentHealth -= 1;

        StartCoroutine(_pickup.GotHurt());
    }

    public void Punched()
    {
        Destroy(gameObject);
    }
}
