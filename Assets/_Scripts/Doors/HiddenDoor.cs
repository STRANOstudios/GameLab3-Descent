using UnityEngine;

public class HiddenDoor : HP
{
    [Header("Audio source")]
    [SerializeField] AudioClip sound;

    private Animator anim;
    private AudioSource audioSource;

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
        audioSource.Stop();
        audioSource.clip = sound;
        audioSource.Play();
    }
}
