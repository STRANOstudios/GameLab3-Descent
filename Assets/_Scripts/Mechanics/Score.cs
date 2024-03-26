using UnityEngine;
using UnityEngine.Audio;

public class Score : MonoBehaviour
{
    [Header("Score settings")]
    [SerializeField] int score = 0;

    [Header("SFX settings")]
    [SerializeField] AudioMixer controller;
    [SerializeField] AudioClip clip;

    public delegate void ObjectDeactivated(int value);
    public static event ObjectDeactivated OnObjectDeactivated;

    void OnDisable()
    {
        OnObjectDeactivated?.Invoke(score);

        if (clip)
        {
            GameObject tempAudioObject = new GameObject("TempAudioSource");
            AudioSource tempAudioSource = tempAudioObject.AddComponent<AudioSource>();
            tempAudioSource.outputAudioMixerGroup = controller.FindMatchingGroups("Master")[2];
            tempAudioSource.PlayOneShot(clip);
            Destroy(tempAudioObject, clip.length);
        }
    }
}
