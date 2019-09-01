using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Reese
{
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
        public float killRadius = 1f;

        //Animation stuff.
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
        }


        void Update()
        {
            KillPlayer();
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
                    if ((currentPlayer.position.x - transform.position.x) < 0) // If player is on the right.
                    {
                        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z));
                        if (!stopShooting)
                        {
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
                        transform.Rotate(transform.rotation.x, transform.rotation.y + 180, transform.rotation.z);
                        stopRotating = true;
                        Invoke("SwapStates", 1f);
                        rotateCount++;
                    }

                    else if (rotateCount == 4)
                    {
                        // Guard goes back to patrolling
                        Patrol();

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

        void SwapStates()
        {
            if(stopRotating)
            {
                stopRotating = false;
            }

            if(stopShooting)
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
            else
            {
                Debug.Log("This should happen");
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
}
