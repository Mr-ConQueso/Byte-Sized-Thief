using UnityEngine;

public class MusicController : MonoBehaviour
{
    // ---- / Private Variables / ---- //
    private AudioSource _musicSource;
    private float _lastPlayedTime;
    
    public void SaveLastPlayedTime()
    {
        _lastPlayedTime = _musicSource.time;
    }
    
    public void PlayAtLastPlayedTime()
    {
        PlayMusicAtTime(_lastPlayedTime);
    }
    
    private void Start()
    {
        _musicSource = GetComponent<AudioSource>();
        PlayAtLastPlayedTime();
    }

    private void PlayMusicAtTime(float timeWanted)
    {
        if (timeWanted <= _musicSource.clip.length)
        {
            _musicSource.time = timeWanted;
            _musicSource.Play();
        }
    }
}
