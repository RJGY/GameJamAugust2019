using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Reese
{

    public class Guard : MonoBehaviour
    {
        private Transform currentPlayer = null;

        private Sprite guardSprite;
        private bool stopRotating = false;

        // Everything for navmesh stuff - patrol
        public Transform waypointParent;
        private Transform[] points;
        public float waypointDistance;
        public float speed = 2f;
        public int currentWayPoint = 1;
        public int rotateCount;

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
        }


        void Update()
        {
            Transform currentPoint = points[currentWayPoint]; // Gets current point.

            if (currentPlayer == null)
            {
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
                    }
                }
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
                        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.x, 180f, transform.rotation.z));
                        if (!stopShooting)
                        {
                            stopShooting = true;
                            Shoot();
                            Invoke("SwapStates", 0.5f);
                        }
                    }
                    else // Player is on the left.
                    {
                        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.x, 0f, transform.rotation.z));
                        if (!stopShooting)
                        {
                            stopShooting = true;
                            Shoot();
                            Invoke("SwapStates", 0.5f);
                        }
                    }
                }

                else
                {
                    // Flip the guard, make it look like he is looking for the alien.
                    if (!stopRotating && rotateCount < 5)
                    {
                        transform.Rotate(transform.rotation.x, transform.rotation.y + 180, transform.rotation.z);
                        stopRotating = true;
                        Invoke("SwapStates", 1f);
                        rotateCount++;
                    }

                    else if (rotateCount >= 5)
                    {
                        // Guard goes back to patrolling
                        transform.position = Vector2.MoveTowards(transform.position, currentPoint.position, speed * Time.deltaTime);
                        rotateCount = 0;
                        currentPlayer = null;
                    }

                    // Guard knows something ie here, but does not know here to look.

                }
            }
        }

        void SwapStates()
        {
            if(stopRotating)
            {
                Debug.Log("Just rotated");
                stopRotating = false;
            }

            if(stopShooting)
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
}
