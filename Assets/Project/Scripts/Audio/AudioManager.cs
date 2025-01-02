using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Audio {
    public class AudioManager : MonoBehaviour {
        [SerializeField, Tooltip("List of categories that hold a number of sounds and their settings.")] private List<SoundCategory> _soundLibrary;
        public List<SoundCategory> SoundLibrary { get => _soundLibrary; }

        private static AudioManager _instance;
        public static AudioManager Instance { get => _instance; private set => _instance = value; }

        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeSounds();
        }

        private void Start() {
            PlaySound("Background Music");
        }

        private void InitializeSounds() {
            foreach (SoundCategory category in SoundLibrary) {
                foreach (Sound sound in category.Sounds) {
                    AudioSource soundObject = new GameObject($"Sound_{category.CategoryName}_{sound.SoundName}").AddComponent<AudioSource>();
                    soundObject.transform.SetParent(transform);
                    sound.SetSource(soundObject);
                }
            }
        }

        public void PlaySound(string soundName, bool waitUntilComplete = false) {
            Sound requestedSound = GetSound(soundName);

            if (requestedSound.LoopPlayback) {
                if (GetSource(soundName).isPlaying) {
                    return;
                }
            }

            requestedSound.Play();
            return;
        }

        public bool IsPlaying(string soundName) {
            AudioSource source = GetSource(soundName);
            if (source == null) {
                Debug.Log($"Could not find source with the given name: {soundName}");
                return false;
            }

            return source.isPlaying;

        }

        public AudioSource GetSource(string soundName) {
            Debug.Assert(!string.IsNullOrEmpty(soundName), "Attempting to stop playing a sound with null or empty name.", this);

            foreach (AudioSource source in transform.GetComponentsInChildren<AudioSource>()) {
                if (!source.name.Contains(soundName)) {
                    continue;
                }

                return source;
            }

            Debug.Log($"Could not find source with the given name: {soundName}");
            return null;
        }

        public Sound GetSound(string soundName) {
            Debug.Assert(!string.IsNullOrEmpty(soundName), "Attempting to play a sound with null or empty name.", this);

            foreach (SoundCategory category in SoundLibrary) {
                foreach (Sound sound in category.Sounds) {
                    if (sound.SoundName != soundName) {
                        continue;
                    }

                    return sound;
                }
            }

            Debug.Log($"Could not find sound with the given name: {soundName}");
            return null;
        }

        public void StopPlaying(string soundName) {
            Debug.Assert(!string.IsNullOrEmpty(soundName), "Attempting to stop playing a sound with null or empty name.", this);

            AudioSource _source = GetSource(soundName);
            _source.Stop();
        }
    }

}