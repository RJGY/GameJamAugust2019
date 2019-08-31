using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHandler : MonoBehaviour
{
    public static bool dead;
    public Animator anim;
    //public Transform deathCameraPos;
    public GameObject gameoverScreen;
    [Header("Health")]
  //  public Slider healthBar;
   // public Image healthFill;
    public int curHealth, maxHealth;
    [Header("Ammo")]
    //public Slider ammoBar;
    public int curAmmo, maxAmmo;
    //[Header("Stamina")]
   // public Slider StaminaBar;
   // public float curStamina, maxStamina;
    
    

    private void Start()
    {
        dead = false;
        //gameoverScreen = false;
    }

    void ResetAnim()
    {
        anim.SetBool("Death", false);
    }
    public void Update()
    {
        if(Input.GetButton("Fire1"))
        {
            //anim.SetTrigger("Attack");

        }
    }
    private void LateUpdate()
    {
       
        if (curHealth <= 0 & !(dead))
        {
            Debug.Log("You have been Exterminated");
            dead = true;
            anim.SetTrigger("Damage");
            anim.SetBool("Death", true);
            Invoke("ResetAnim", 0.25f);
            
            
        }

        else if (dead)
        {
            
        }
        else
        {

        }
    }
    

    //private void OnCollisionEnter(Collision other)
    //{
        //if(other.gameObject.tag == "Arrow")
        //{
            //curHealth -=
                //other.gameObject.GetComponent<ArrowHandler>().damage;
            //anim.SetTrigger("Damage");
        //}
        //if (other.gameObject.layer == "Hazard")
        //{
          //  curHealth -= 1;
                
        //}

    //}
}
