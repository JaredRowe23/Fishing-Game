using UnityEngine;

namespace Fishing.Audio {
    [System.Serializable]
    public class Sound {
        [SerializeField, Tooltip("Name of this sound.")] private string _soundName = "New Sound";
        public string SoundName { get => _soundName; set => _soundName = value; }
        [SerializeField, Tooltip("Audio clip of this sound.")] private AudioClip _soundClip;
        public AudioClip SoundClip { get => _soundClip; set => _soundClip = value; }

        [SerializeField, Tooltip("Whether or not to loop this sound during playback.")] private bool _loopPlayback;
        public bool LoopPlayback { get => _loopPlayback; set => _loopPlayback = value; }

        [SerializeField, Range(0, 2), Tooltip("Volume of this sound during playback. Applied as a percentage of the audio clip's original volume.")] private float _volume = 0.7f;
        public float Volume { get => _volume; set => _volume = value; }
        [SerializeField, Range(0.5f, 1.5f), Tooltip("Pitch of this sound during playback. Applied as a percentage of the audio clip's original pitch.")] private float _pitch = 1f;
        public float Pitch { get => _pitch; set => _pitch = value; }

        [SerializeField, Range(0, 0.5f), Tooltip("Range of variability in this sound's volume when played. Applied as a percentage in both directions of volume variable (set to 0.1 of 1 volume results in range of 0.95 - 1.05).")] private float _randomVolumeRange = 0.1f;
        public float RandomVolumeRange { get => _randomVolumeRange; set => _randomVolumeRange = value; }
        [SerializeField, Range(0, 0.5f), Tooltip("Range of variability in this sound's pitch when played. Applied as a percentage in both directions of pitch variable (set to 0.1 of 1 pitch results in range of 0.95  -1.05).")] private float _randomPitchRange = 0.1f;
        public float RandomPitchRange { get => _randomPitchRange; set => _randomPitchRange = value; }

        private AudioSource _source;

        public void SetSource(AudioSource source) {
            _source = source;
            _source.clip = SoundClip;
        }

        public void Play() {
            _source.volume = Volume * (1 + Random.Range(-RandomVolumeRange * 0.5f, RandomVolumeRange * 0.5f));
            _source.pitch = Pitch * (1 + Random.Range(-RandomPitchRange * 0.5f, RandomPitchRange * 0.5f));
            _source.loop = LoopPlayback;
            _source.Play();
        }
    }
}
