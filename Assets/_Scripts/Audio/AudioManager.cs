using System.Collections.Generic;
using UnityEngine;

namespace epoHless
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private List<AudioClass> audioClasses;

        /// <summary>
        /// Initialise the classes in start, since the AudioMixer is not available in Awake
        /// </summary>
        private void Start()
        {
            foreach (var audioClass in audioClasses)
            {
                audioClass.Initialize();
            }
        }

        private void OnEnable()
        {
            foreach (var audioClass in audioClasses)
            {
                audioClass.Listen();
            }
        }
        
        private void OnDisable()
        {
            foreach (var audioClass in audioClasses)
            {
                audioClass.Unlisten();
            }
        }
    }
}
