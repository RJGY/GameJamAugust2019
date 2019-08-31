﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Change this value later to the player script.
    private Transform currentPlayer;

    private float setDistance = 9f;
    private float bulletSpeed = 2f;

    private bool movingToRight = false;
    private bool movingToLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        // Change this later on to the player script.
        currentPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if ((currentPlayer.position.x - transform.position.x) < 0) // If player is on the left.
        {
            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90f));
            movingToRight = true;
        }

        else
        {
            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 270f));
            movingToLeft = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(movingToRight)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x - 9, transform.position.y), bulletSpeed * Time.deltaTime);
        }
        else if (movingToLeft)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + 9, transform.position.y), bulletSpeed * Time.deltaTime);
        }
    }
}
