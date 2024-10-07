using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Pool;

public class AudioController : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static AudioController Instance;
    
    // ---- / Public Variables / ---- //
    public IObjectPool<SoundEmitter> SoundEmitterPool;
    public readonly List<SoundEmitter> activeSoundEmitter = new();
    public readonly Dictionary<SoundData, int> Counts = new();
    public readonly Queue<SoundEmitter> FrequentSoundEmitters = new();
    
    // ---- / Serialized Variables / ---- //
    [SerializeField] private SoundEmitter soundEmitterPrefab;
    [SerializeField] private bool collectionCheck = true;
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxPoolSize = 100;
    [SerializeField] private int maxSoundInstances = 30;
    
    // ---- / Private Variables / ---- //
    private AudioSource _audioSource;
    private float _lastPlayedTime;

    public SoundBuilder CreateSound() => new SoundBuilder(this);
    
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
    }

    public bool CanPlaySound(SoundData data)
    {
        if (Counts.TryGetValue(data, out var count))
        {
            return false;
        }
        return true;

        if (!data.frequentSound) return true;
        
        
    }

    public SoundEmitter Get()
    {
        return SoundEmitterPool.Get();
    }

    public void ReturnToPool(SoundEmitter soundEmitter)
    {
        SoundEmitterPool.Release(soundEmitter);
    }

    private void OnDestroyPoolObject(SoundEmitter soundEmitter)
    {
        Destroy(soundEmitter);
    }

    private void OnReturnedToPool(SoundEmitter soundEmitter)
    {
        if (Counts.TryGetValue(soundEmitter.Data, out var count))
        {
            Counts[soundEmitter.Data] -= count > 0 ? 1 : 0;
        }
        soundEmitter.gameObject.SetActive(false);
        activeSoundEmitter.Remove(soundEmitter);
    }

    private void OnTakeFromPool(SoundEmitter soundEmitter)
    {
        soundEmitter.gameObject.SetActive(true);
        activeSoundEmitter.Add(soundEmitter);
    }

    private SoundEmitter CreateSoundEmitter()
    {
        var soundEmitter = Instantiate(soundEmitterPrefab);
        soundEmitterPrefab.gameObject.SetActive(false);
        return soundEmitter;
    }

    private void InitializePool()
    {
        SoundEmitterPool = new ObjectPool<SoundEmitter>(
            CreateSoundEmitter,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            collectionCheck,
            defaultCapacity,
            maxPoolSize);
    }
}
