using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static AudioController Instance;
    
    // ---- / Public Variables / ---- //
    public IObjectPool<SoundEmitter> SoundEmitterPool;
    public readonly List<SoundEmitter> ActiveSoundEmitter = new();
    public readonly Queue<SoundEmitter> FrequentSoundEmitters = new();
    
    // ---- / Serialized Variables / ---- //
    [SerializeField] private SoundEmitter soundEmitterPrefab;
    [SerializeField] private bool collectionCheck = true;
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxPoolSize = 100;
    [SerializeField] private int maxSoundInstances = 30;

    public SoundBuilder CreateSound() => new SoundBuilder(this);

    public bool CanPlaySound(SoundData data)
    {
        if (!data.frequentSound) return true;

        if (FrequentSoundEmitters.Count >= maxSoundInstances && FrequentSoundEmitters.TryDequeue(out var soundEmitter))
        {
            try
            {
                soundEmitter.Stop();
                return true;
            }
            catch
            {
                Debug.Log("SoundEmitter is already released");
            }

            return false;
        }

        return true;
    }

    public SoundEmitter Get()
    {
        return SoundEmitterPool.Get();
    }

    public void ReturnToPool(SoundEmitter soundEmitter)
    {
        SoundEmitterPool.Release(soundEmitter);
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        InitializePool();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        for (int i = 0; i < ActiveSoundEmitter.Count; i++)
        {
            if (ActiveSoundEmitter[i].gameObject != null)
            {
                Destroy(ActiveSoundEmitter[i].gameObject);
            }
        }
        ActiveSoundEmitter.Clear();
    }

    private void OnDestroyPoolObject(SoundEmitter soundEmitter)
    {
        Destroy(soundEmitter);
    }

    private void OnReturnedToPool(SoundEmitter soundEmitter)
    {
        soundEmitter.gameObject.SetActive(false);
        ActiveSoundEmitter.Remove(soundEmitter);
    }

    private void OnTakeFromPool(SoundEmitter soundEmitter)
    {
        soundEmitter.gameObject.SetActive(true);
        ActiveSoundEmitter.Add(soundEmitter);
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
