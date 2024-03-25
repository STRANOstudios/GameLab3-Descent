using UnityEngine;

public class HiddenDoor : HP
{
    [Header("Audio source")]
    [SerializeField] AudioSource audioSource;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);

        if (collision.gameObject.layer == 13)
        {
            myHP -= collision.gameObject.GetComponent<Projectile>().GetDamage;

            if (myHP <= 0) Death();
        }
    }

    public override void Death()
    {
        if (audioSource) audioSource.Play();
        anim.SetBool("OpenDoor", true);
        GetComponent<BoxCollider>().enabled = false;
        //Destroy(gameobject);  maybe???????
    }
}
