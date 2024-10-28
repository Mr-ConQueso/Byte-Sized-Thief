
using UnityEngine;

namespace Audio
{
    public class SoundBuilder
    {
        // ---- / Private Variables / ---- //
        private readonly AudioController _audioController;
        private SoundData _soundData;
        private Vector3 _position = Vector3.zero;
        private bool _randomPitch;

        public SoundBuilder(AudioController audioController)
        {
            this._audioController = audioController;
        }

        public SoundBuilder WithSoundData(SoundData soundData)
        {
            this._soundData = soundData;
            return this;
        }
        
        public SoundBuilder WithRandomPitch(bool useRandomPitch = true)
        {
            this._randomPitch = useRandomPitch;
            return this;
        }
        
        public SoundBuilder WithPosition(Vector3 position)
        {
            this._position = position;
            return this;
        }

        public void Play()
        {
            if (!_audioController.CanPlaySound(_soundData)) return;

            SoundEmitter soundEmitter = _audioController.Get();
            soundEmitter.Initialize(_soundData);
            soundEmitter.transform.position = _position;
            soundEmitter.transform.parent = AudioController.Instance.transform;

            if (_randomPitch)
            {
                soundEmitter.WithRandomPitch();
            }

            if (_soundData.frequentSound)
            {
                _audioController.FrequentSoundEmitters.Enqueue(soundEmitter);
            }
            
            soundEmitter.Play();
        }
    }
}