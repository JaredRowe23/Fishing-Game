using UnityEngine;

namespace Fishing
{
    [System.Serializable]
    public class Sound
    {

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

        public void SetSource(AudioSource _source)
        {
            source = _source;
            source.clip = clip;
        }

        public void Play()
        {
            source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
            source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
            source.loop = loop;
            source.Play();
        }
    }

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        [SerializeField]
        Sound[] gameplay;

        [SerializeField]
        Sound[] UI;

        [SerializeField]
        Sound[] music;

        Sound[][] soundArrays = new Sound[3][];


        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            soundArrays[0] = gameplay;
            soundArrays[1] = UI;
            soundArrays[2] = music;
            for (int i = 0; i < soundArrays.Length; i++)
            {
                for (int j = 0; j < soundArrays[i].Length; j++)
                {
                    GameObject _go = new GameObject("Sound_" + i + "_" + soundArrays[i][j].name);
                    _go.transform.SetParent(transform);
                    soundArrays[i][j].SetSource(_go.AddComponent<AudioSource>());
                }
            }
            PlaySound("Background Music");
        }

        public void PlaySound(string _name, bool waitUntilComplete = false)
        {
            Sound requestedSound = GetSound(_name);

            if (requestedSound.loop)
            {
                if (GetSource(_name).isPlaying)
                {
                    return;
                }
            }

            requestedSound.Play();
            return;
        }

        public bool IsPlaying(string _name)
        {
            AudioSource _source = GetSource(_name);
            if (_source == null)
            {
                Debug.Log("Could not find source with the given name: " + _name);
                return false;
            }
            return _source.isPlaying;

        }

        public AudioSource GetSource(string _name)
        {
            foreach (AudioSource _source in transform.GetComponentsInChildren<AudioSource>())
            {
                if (!_source.name.Contains(_name))
                {
                    continue;
                }
                return _source;
            }
            Debug.Log("Could not find source with the given name: " + _name);
            return null;
        }

        public Sound GetSound(string _name)
        {
            for (int i = 0; i < soundArrays.Length; i++)
            {
                for (int j = 0; j < soundArrays[i].Length; j++)
                {
                    if (soundArrays[i][j].name != _name) continue;
                    return soundArrays[i][j];
                }
            }

            Debug.Log("Could not find sound with the given name: " + _name);
            return null;
        }

        public void StopPlaying(string _name)
        {
            AudioSource _source = GetSource(_name);
            //for (int i = 0; i > sounds.Length; i++)
            //{
            //    if (sounds[i].name != _name)
            //    {
            //        continue;
            //    }
            //}
            _source.Stop();
        }
    }

}