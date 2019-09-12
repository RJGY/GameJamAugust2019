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
            guard.anim.SetTrigger("Dying");
            Debug.Log("Just hit a guard");
            SoundManager.Instance.PlaySound("Man Death");
            guard.gameObject.SetActive(false);
        }

        else if (runner != null)
        {
            runner.Anim.SetTrigger("Dying");
            Debug.Log("Just hit a runner");
            SoundManager.Instance.PlaySound("Death of dog");
            runner.gameObject.SetActive(false);
        }

        else if (turret != null)
        {
            SoundManager.Instance.PlaySound("Turret destroyed");
            turret.anim.SetTrigger("Dying");
            Debug.Log("Just hit a turret");
            turret.gameObject.SetActive(false);
        }


        // After death animation, destroy the whole gameoject
        Invoke("DestroyAll", 1f);

    }

    void DestroyAll()
    {
        gameObject.SetActive(false);
    }
}

