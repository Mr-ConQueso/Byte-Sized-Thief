using System;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private Vector2 musicSpeed = new Vector2(1, 3);
    [SerializeField] private float maxTime = 120f;
    
    // ---- / Private Variables / ---- //
    private AudioSource _musicSource;
    private float _lastPlayedTime;
    private float currentTime = 0f;
    private float currentMusicSpeed;
    
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
    
    /*
    private void Update()
    {
        currentTime += Time.deltaTime;

        currentMusicSpeed = Mathf.Lerp(musicSpeed.x, musicSpeed.y, currentTime / maxTime);

        _musicSource.pitch = currentMusicSpeed;
        mainMixer.SetFloat("MusicPitch", 1f / currentMusicSpeed);
    
        currentTime = Mathf.Clamp(currentTime, 0, maxTime);
    }
    */

    private void PlayMusicAtTime(float timeWanted)
    {
        if (timeWanted <= _musicSource.clip.length)
        {
            _musicSource.time = timeWanted;
            _musicSource.Play();
        }
    }
}
