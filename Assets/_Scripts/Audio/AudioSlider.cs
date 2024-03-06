using UnityEngine;
using UnityEngine.UI;

namespace epoHless
{
    [RequireComponent(typeof(Slider))]
    public class AudioSlider : MonoBehaviour
    {
        /// <summary>
        /// Wrapper delegate for the slider, it's a step to avoid the direct use of the slider
        /// </summary>
        public delegate void AudioCallback(float volume);
        private AudioCallback OnVolumeChanged;

        private Slider slider;

        public float Volume => slider.value; // Getter *Property* for the volume

        private void Awake()
        {
            slider = GetComponent<Slider>();

            // Set the default values for the slider(dB)
            slider.minValue = -80;
            slider.maxValue = 20;
        }

        private void OnEnable()
        {
            slider.onValueChanged.AddListener(ChangeVolume);
        }

        private void OnDisable()
        {
            slider.onValueChanged.RemoveListener(ChangeVolume);
        }

        void ChangeVolume(float value)
        {
            OnVolumeChanged?.Invoke(value);
        }

        public void AddListener(AudioCallback callback)
        {
            OnVolumeChanged += callback;
        }

        public void RemoveListener(AudioCallback callback)
        {
            OnVolumeChanged -= callback;
        }

        public void SetVolume(float volume)
        {
            slider.value = volume;
        }
    }
}
