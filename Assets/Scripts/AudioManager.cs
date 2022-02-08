using UnityEngine;

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
    Sound[] sounds;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one AudioManager in the scene.");
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.SetParent(transform);
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }
        PlaySound("Background Music");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StopPlaying("Reel");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            PlaySound("Reel");
        }
    }

    public void PlaySound(string _name, bool waitUntilComplete = false)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name != _name)
            {
                continue;
            }

            if (sounds[i].loop)
            {
                if(GetSource(_name).isPlaying)
                {
                    return;
                }
            }

            sounds[i].Play();
            return;
        }

        //no sound with name
        Debug.LogWarning("AudioManager: Sound not found in list, " + _name);
    }

    public bool IsPlaying(string _name)
    {
        AudioSource source = GetSource(_name);
        if (source == null)
        {
            Debug.Log("Could not find source with the given name: " + _name);
            return false;
        }
        return source.isPlaying;
        
    }

    public AudioSource GetSource(string _name)
    {
        foreach (AudioSource source in transform.GetComponentsInChildren<AudioSource>())
        {
            if (!source.name.Contains(_name))
            {
                continue;
            }
            return source;
        }
        Debug.Log("Could not find source with the given name: " + _name);
        return null;
    }

    public Sound GetSound(string _name)
    {
        foreach(Sound sound in sounds)
        {
            if (sound.name != _name) continue;
            return sound;
        }
        Debug.Log("Could not find sound with the given name: " + _name);
        return null;
    }

    public void StopPlaying(string _name)
    {
        AudioSource source = GetSource(_name);
        for (int i = 0; i > sounds.Length; i++)
        {
            if (sounds[i].name != _name)
            {
                continue;
            }
        }
        source.Stop();
    }
}
