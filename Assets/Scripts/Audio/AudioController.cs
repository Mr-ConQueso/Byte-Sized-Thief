using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static AudioController Instance;
    
    // ---- / Serialized Variables / ---- //
    [SerializeField] private AudioMixer mainMixer;
    
    // ---- / Private Variables / ---- //
    private AudioSource _audioSource;
    private float _lastPlayedTime;
    
    public void SaveLastPlayedTime()
    {
        _lastPlayedTime = _audioSource.time;
    }
    
    public void PlayAtLastPlayedTime()
    {
        PlayMusicAtTime(_lastPlayedTime);
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        PlayAtLastPlayedTime();
    }

    private void PlayMusicAtTime(float timeWanted)
    {
        if (timeWanted <= _audioSource.clip.length)
        {
            _audioSource.time = timeWanted;
            _audioSource.Play();
        }
    }
}
