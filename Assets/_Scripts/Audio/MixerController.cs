using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MixerController : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mixer;

    [Header("Audio Slider")]
    [SerializeField] Slider masterSlider = null;
    [SerializeField] Slider musicSlider = null;
    [SerializeField] Slider sfxSlider = null;
    [SerializeField] Slider dialogueSlider = null;

    private void Start()
    {
        SetMixerVolumes();
    }

    void SetMixerVolumes()
    {
        mixer.SetFloat("Master", GetSavedFloat("Master"));

        mixer.SetFloat("Sfx", GetSavedFloat("Sfx"));

        mixer.SetFloat("Music", GetSavedFloat("Music"));

        mixer.SetFloat("Dialogue", GetSavedFloat("Dialogue"));
    }

    public void SetSliderVolumes()
    {
        masterSlider.value = GetSavedFloat("Master");

        sfxSlider.value = GetSavedFloat("Sfx");

        musicSlider.value = GetSavedFloat("Music");

        dialogueSlider.value = GetSavedFloat("Dialogue");
    }

    public void SetVolume(Slider slider)
    {
        mixer.SetFloat(slider.name, slider.value);

        PlayerPrefs.SetFloat($"{slider.name}", slider.value);
        PlayerPrefs.Save();
    }

    float GetSavedFloat(string key)
    {
        if (PlayerPrefs.HasKey(key)) return PlayerPrefs.GetFloat(key);
        return 0f;
    }
}
