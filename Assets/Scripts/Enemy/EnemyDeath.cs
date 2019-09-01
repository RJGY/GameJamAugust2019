using UnityEngine;



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

        if (triggerEvent != null)
        {
            Destroy(triggerEvent);
        }


        if (guard != null)
        {
            // Play respective enemy death animation.
            Destroy(guard);
        }

        else if (runner != null)
        {
            // Play respective enemy death animation.
            Destroy(runner);
        }

        else if (turret != null)
        {
            // Play respective enemy death animation.
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

