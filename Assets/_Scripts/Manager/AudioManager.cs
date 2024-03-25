using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioClip bossBattle;
    [SerializeField] AudioClip countdown;
    [SerializeField] AudioClip escaped;
    [SerializeField] AudioClip death;

    private AudioSource AudioSource;

    private void Start()
    {
        AudioSource = GameManager.Instance.GetComponent<AudioSource>();

        if (!AudioSource) enabled = false;
    }

    private void OnEnable()
    {
        Door.bossfight += PlayBossBattle;
        CoreLogic.death += PlayCountdown;
        Escaped.escaped += Escape;
        HealthManager.dead += Gameover;
    }

    private void OnDisable()
    {
        Door.bossfight -= PlayBossBattle;
        CoreLogic.death -= PlayCountdown;
        Escaped.escaped -= Escape;
        HealthManager.dead -= Gameover;
    }

    private void Gameover()
    {
       SetClip(death);
    }

    private void Escape()
    {
        SetClip(escaped);
    }

    private void PlayCountdown()
    {
        SetClip(countdown);
    }

    void PlayBossBattle()
    {
        SetClip(bossBattle);
    }

    void SetClip(AudioClip clip)
    {
        if (!clip) return;
        AudioSource.Stop();
        AudioSource.clip = clip;
        AudioSource.Play();
    }
}
