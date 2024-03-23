using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenDoor : HP
{
    float damageTaken;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 13)
        {
            damageTaken = other.GetComponent<Projectile>().GetDamage;
            myHP -= damageTaken; 

            if(myHP <= 0 )
            {
                myHP = 0;
                Death();
            }
        }
    }

    public override void Death()
    {
        anim.SetBool("OpenDoor", true);
        //Destroy(gameobject);  maybe???????
    }
}
