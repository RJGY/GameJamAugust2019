using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pickup : MonoBehaviour
{
    private bool Invincible = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
       
        int healthCount = gameObject.GetComponent<PlayerHandler1>().curHealth / gameObject.GetComponent<PlayerHandler1>().maxHealth;
        if (other.tag == "Health" && healthCount < 1)
        {
            gameObject.GetComponent<PlayerHandler1>().curHealth += 1;
            Destroy(other.gameObject);
        }
        if (other.tag == "DangerZone" && !Invincible)
        {
            gameObject.GetComponent<PlayerHandler1>().curHealth -= 1;
            Destroy(other.gameObject);
            StartCoroutine(GotHurt());
        }
        if (other.tag == "Spike" && !Invincible)
        {
            
            gameObject.GetComponent<PlayerHandler1>().curHealth -= 1;
            StartCoroutine(GotHurt());
        }

        
        
        if (other.tag == "Typewriter")
        {
            PlayerPrefs.Save();
        }
        

    }
    public IEnumerator GotHurt()
    {
        Invincible = true;
        yield return new WaitForSeconds (3);
        Invincible = false;

    }
    

}
