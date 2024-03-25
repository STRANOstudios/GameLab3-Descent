using UnityEngine;

public class Score : MonoBehaviour
{
    [Header("Score settings")]
    [SerializeField] int score = 0;

    [Header("Audio source")]
    [SerializeField] AudioSource audioSource;

    public delegate void ObjectDeactivated(int value);
    public static event ObjectDeactivated OnObjectDeactivated;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnDisable()
    {
        OnObjectDeactivated?.Invoke(score);
        if (audioSource != null) audioSource.Play();
    }
}
