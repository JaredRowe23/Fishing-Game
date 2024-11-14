using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing {
    [System.Serializable]
    public class Sound {

        public string name = "New Sound";
        public AudioClip clip;

        public bool loop;

        [Range(0, 1)]
        public float volume = 0.7f;
        [Range(0.5f, 1.5f)]
        public float pitch = 1f;

        [Range(0, 0.5f)]
        public float randomVolume = 0.1f;
        [Range(0, 0.5f)]
        public float randomPitch = 0.1f;

        private AudioSource source;

        public void SetSource(AudioSource _source) {
            source = _source;
            source.clip = clip;
        }

        public void Play() {
            source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
            source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
            source.loop = loop;
            source.Play();
        }
    }
}
