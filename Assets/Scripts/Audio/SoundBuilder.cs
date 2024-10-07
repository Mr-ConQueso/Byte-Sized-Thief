
using UnityEngine;

namespace Audio
{
    public class SoundBuilder
    {
        public readonly AudioController AudioController;
        private SoundData SoundData;
        private Vector3 position = Vector3.zero;
        private bool randomPitch;

        public SoundBuilder(AudioController audioController)
        {
            this.AudioController = audioController;
        }

        public SoundBuilder WithSoundData(SoundData soundData)
        {
            this.SoundData = soundData;
            return this;
        }
        
        public SoundBuilder WithRandomPitch()
        {
            this.randomPitch = true;
            return this;
        }
        
        public SoundBuilder WithPosition(Vector3 position)
        {
            this.position = position;
            return this;
        }

        public void Play()
        {
            if (!AudioController.CanPlaySound(SoundData)) return;

            SoundEmitter soundEmitter = AudioController.Get();
            soundEmitter.Initialize(SoundData);
            soundEmitter.transform.position = position;
            soundEmitter.transform.parent = AudioController.Instance.transform;

            if (randomPitch)
            {
                soundEmitter.WithRandomPitch();
            }

            if (AudioController.Counts.TryGetValue(SoundData, out var count))
            {
                AudioController.Counts[SoundData] = count + 1;
            }
            else
            {
                AudioController.Counts[SoundData] = 1;
            }
            soundEmitter.Play();
        }
    }
}