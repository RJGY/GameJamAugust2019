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

        // Everything for navmesh stuff - patrol
        public Transform waypointParent;
        private Transform[] points;
        public float waypointDistance;
        public float speed = 3f;
        public NavMeshAgent agent;
        public int currentWayPoint = 1;

        // Everything for facing player and shooting
        private float horizontalDistance = 9f;
        private float verticalDistance = 1f;



        void Start()
        {
            points = waypointParent.GetComponentsInChildren<Transform>();
            guardSprite = GetComponent<Sprite>();
            Patrol();
        }


        void Update()
        {
            if (currentPlayer == null)
            {
                Transform currentPoint = points[currentWayPoint]; // Gets current point.

                float distance = Vector2.Distance(transform.position, currentPoint.position); // Finds distance between itself and the current point.

                transform.position = Vector3.MoveTowards(transform.position, currentPoint.position, speed * Time.deltaTime);

                if (distance < waypointDistance)
                {
                    if (currentWayPoint < points.Length - 1)
                    {
                        currentWayPoint++;
                    }

                    else
                    {// if current waypoints is outside array length
                     // reset back to 1
                        currentWayPoint = 1;
                    }
                }
            }

            else
            {
                // Guard hears you.
                float checkHorizontalDistance = Mathf.Abs(currentPlayer.position.x - transform.position.x);
                float checkVerticalDistance = Mathf.Abs(currentPlayer.position.y - transform.position.y);

                if (checkHorizontalDistance > horizontalDistance && checkVerticalDistance > verticalDistance)
                {
                    // Player has been detected.
                }

                else
                {
                    // Flip the guard, make it look like he is looking for the alien.
                    // Guard knows something ie here, but does not know here to look.
                }
            }
        }

        void Patrol()
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
