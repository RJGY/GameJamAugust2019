using UnityEngine;

namespace Reese
{


    public class EnemyDeath : MonoBehaviour
    {
        public bool enemyDead = false;
        public Animator anim;
        private Guard guard;
        private Runner runner;
        private Turret turret;
        private TriggerEvent triggerEvent;
        // Start is called before the first frame update
        void Start()
        {
            triggerEvent = GetComponent<TriggerEvent>();
            guard = GetComponent<Guard>();
            runner = GetComponent<Runner>();
            turret = GetComponent<Turret>();
        }

        // Update is called once per frame
        void Update()
        {
            if (enemyDead)
            {
                OnDeath();
            }
        }

        void OnDeath()
        {

            // Play respective enemy death animation.
            // if (guard)
            // anim.DeathAnimation

            Destroy(triggerEvent);

            if (guard != null)
            {
                Destroy(guard);
            }

            else if (runner != null)
            {
                Destroy(runner);
            }

            else if (turret != null)
            {
                Destroy(turret);
            }

            
            // After death animation, destroy the whole gameoject
            Invoke("DestroyAll", 3f);

        }

        void DestroyAll()
        {
            gameObject.SetActive(false);
        }
    }
}

