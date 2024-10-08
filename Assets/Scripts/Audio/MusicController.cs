using UnityEngine;

public class MusicController : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private SoundData musicData;
    
    private void Start()
    {
        AudioController.Instance.CreateSound()
            .WithSoundData(musicData)
            .WithPosition(this.transform.position)
            .Play();
    }
}
