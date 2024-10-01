using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private AudioClip grabSound;
    [SerializeField] private AudioClip dropSound;
    [SerializeField] private AudioClip sellSound;
    
    // ---- / Private Variables / ---- //
    private AudioSource _audioSource;

    public void Play_GrabSound()
    {
        _audioSource.PlayOneShot(grabSound);
    }
    
    public void Play_DropSound()
    {
        _audioSource.PlayOneShot(dropSound);
    }
    
    public void Play_SellSound()
    {
        _audioSource.PlayOneShot(sellSound);
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
}
