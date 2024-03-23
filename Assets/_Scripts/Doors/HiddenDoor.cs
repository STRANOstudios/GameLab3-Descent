using UnityEngine;

public class HiddenDoor : HP
{
    float damageTaken;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 13)
        {
            Debug.Log("si");
            damageTaken = collision.gameObject.GetComponent<Projectile>().GetDamage;
            myHP -= damageTaken;

            if (myHP <= 0)
            {
                myHP = 0;
                Death();
                gameObject.GetComponent<Collider>().enabled = false;
            }
        }
    }

    public override void Death()
    {
        anim.SetBool("OpenDoor", true);
        //Destroy(gameobject);  maybe???????
    }
}
