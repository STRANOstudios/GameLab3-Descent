using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    /// <summary>
    /// Add as many audio types as you want
    /// </summary>
    public enum EAudioType
    {
        Master,
        Music,
        Sfx,
        Dialogue
    }

    /// <summary>
    /// Wrapper class for the AudioMixerGroup and AudioSlider
    /// </summary>
    [System.Serializable]
    public class AudioClass
    {
        [SerializeField] private EAudioType AudioType;
        [SerializeField] private AudioSlider AudioSlider;
        [SerializeField] private AudioMixerGroup AudioMixerGroup;

        public void Initialize()
        {
            if (PlayerPrefs.HasKey($"AudioVolume_{AudioType.ToString()}") && AudioSlider != null && AudioMixerGroup != null)
            {
                var volume = PlayerPrefs.GetFloat($"AudioVolume_{AudioType.ToString()}");

                AudioMixerGroup.audioMixer.SetFloat(AudioType.ToString(), volume);
                AudioSlider.SetVolume(volume);
            }
        }

        public void Listen()
        {
            AudioSlider.AddListener(OnVolumeChanged); ;
        }

        public void Unlisten()
        {
            AudioSlider.RemoveListener(OnVolumeChanged);
        }

        void OnVolumeChanged(float volume)
        {
            AudioMixerGroup.audioMixer.SetFloat(AudioType.ToString(), AudioSlider.Volume);
            PlayerPrefs.SetFloat($"AudioVolume_{AudioType.ToString()}", AudioSlider.Volume);
        }
    }
}
