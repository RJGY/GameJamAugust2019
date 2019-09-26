using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pickup : MonoBehaviour
{
    private bool Invincible = false;
    private PlayerHandler _playerHandler;
    private void OnTriggerEnter2D(Collider2D other)
    {
       
        float healthCount = _playerHandler.currentHealth / _playerHandler.maxHealth;
        if (other.tag == "Health" && healthCount < 1)
        {
            _playerHandler.currentHealth += 1;
            Destroy(other.gameObject);
        }
        if (other.tag == "DangerZone" && !Invincible)
        {
            _playerHandler.currentHealth -= 1;
            Destroy(other.gameObject);
            StartCoroutine(GotHurt());
        }
        if (other.tag == "Spike" && !Invincible)
        {

            _playerHandler.currentHealth -= 1;
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
