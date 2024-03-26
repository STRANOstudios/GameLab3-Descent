using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using static GameManager;

public class Score : MonoBehaviour
{
    [Header("Score settings")]
    [SerializeField] int score = 0;

    [Header("SFX settings")]
    [SerializeField] AudioMixer controller;
    [SerializeField] AudioClip clip;

    public delegate void ObjectDeactivated(int value);
    public static event ObjectDeactivated OnObjectDeactivated;

    private bool isSceneUnloading = false;

    private void OnEnable()
    {
        Escaped.escaped += OnSceneUnloaded;
        HealthManager.dead += OnSceneUnloaded;
        LevelManager.quit += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        Escaped.escaped -= OnSceneUnloaded;
        HealthManager.dead -= OnSceneUnloaded;
        LevelManager.quit -= OnSceneUnloaded;

        OnObjectDeactivated?.Invoke(score);

        if (!isSceneUnloading)
        {
            OnObjectDeactivated?.Invoke(score);

            if (clip)
            {
                GameObject tempAudioObject = new("TempAudioSource");
                AudioSource tempAudioSource = tempAudioObject.AddComponent<AudioSource>();
                tempAudioSource.outputAudioMixerGroup = controller.FindMatchingGroups("Master")[2];
                tempAudioSource.PlayOneShot(clip);
                Destroy(tempAudioObject, clip.length);
            }
        }
    }

    private void OnSceneUnloaded()
    {
        isSceneUnloading = true;
    }
}
