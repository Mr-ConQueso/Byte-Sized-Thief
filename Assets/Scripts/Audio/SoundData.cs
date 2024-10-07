using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class SoundData
{
    public AudioClip Clip;
    public AudioMixerGroup MixerGroup;
    public bool loop;
    public bool playOnAwake;
    public bool frequentSound;
}