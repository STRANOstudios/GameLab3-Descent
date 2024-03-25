using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource bossBattle;
    [SerializeField] AudioSource countdown;
    [SerializeField] AudioSource escpaed;

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
        if (escpaed)
        {
            StopAll();
            escpaed.Play();
        }
    }

    private void Escape()
    {
        if (escpaed)
        {
            StopAll();
            escpaed.Play();
        }
    }

    private void PlayCountdown()
    {
        if (bossBattle && countdown)
        {
            bossBattle.Stop();
            countdown.Play();
        }
    }

    void PlayBossBattle()
    {
        if (bossBattle && GameManager.Instance.GetComponent<AudioSource>())
        {
            GameManager.Instance.GetComponent<AudioSource>().Stop();
            bossBattle.Play();
        }
    }

    void StopAll()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            AudioSource[] audioSources = obj.GetComponents<AudioSource>();

            foreach (AudioSource audioSource in audioSources)
            {
                audioSource.enabled = false;
            }
        }
    }
}
