using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [Header("Audio source")]
    [SerializeField] AudioClip sound;

    Animator anim;
    bool canOpen;
    private AudioSource audioSource;

    private void OnEnable()
    {
        CoreLogic.death += Timer;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnDisable()
    {
        CoreLogic.death -= Timer;
    }
    private void Timer()
    {
        canOpen = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canOpen)
        {
            if (audioSource) audioSource.Play();
            anim.SetBool("OpenDoor", true);
            audioSource.Stop();
            audioSource.clip = sound;
            audioSource.Play();
        }
    }
}
