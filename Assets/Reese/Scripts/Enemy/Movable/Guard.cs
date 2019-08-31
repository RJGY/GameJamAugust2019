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

        // For gun.
        public GameObject bulletPrefab;
        public bool stopShooting = false;

        // Everything for facing player and shooting
        private float horizontalDistance = 9f;
        private float verticalDistance = 1f;



        void Start()
        {
            points = waypointParent.GetComponentsInChildren<Transform>();
            guardSprite = GetComponent<Sprite>();
        }


        void Update()
        {
            if (currentPlayer == null)
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
                    {// if current waypoints is outside array length
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
                        // transform.Rotate(transform.rotation.x, 180, transform.rotation.z);
                        if (!stopShooting)
                        {
                            stopShooting = true;
                            Shoot();
                            Invoke("SwapStates", 0.3f);
                        }
                    }
                    else // Player is on the left.
                    {
                        // transform.Rotate(transform.rotation.x, 0, transform.rotation.z);

                    }
                }

                else
                {
                    // Flip the guard, make it look like he is looking for the alien.
                    if (!stopRotating)
                    {
                        transform.Rotate(transform.rotation.x, transform.rotation.y + 180, transform.rotation.z);
                        stopRotating = true;
                        Invoke("SwapStates", 1f);
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
