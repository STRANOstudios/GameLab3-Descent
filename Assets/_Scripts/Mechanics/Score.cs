using UnityEngine;

public class Score : MonoBehaviour
{
    [Header("Score settings")]
    [SerializeField] int score = 0;

    [Header("Audio source")]
    [SerializeField] AudioClip clip;

    private AudioSource AudioSource;

    public delegate void ObjectDeactivated(int value);
    public static event ObjectDeactivated OnObjectDeactivated;

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    void OnDisable()
    {
        OnObjectDeactivated?.Invoke(score);
        if (clip && AudioSource) AudioSource.PlayOneShot(clip);
    }
}
