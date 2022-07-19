using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;

public class SFXManager : Singleton<SFXManager>
{
    public List<AudioClip> playerSFX;
    public AudioSource playerAudioSource;
    public AudioClip _currClipIndex;
    public int clipIndex;

    protected override void Awake()
    {
        base.Awake();
    }

    public void PlayPlayerSFX(int i)
    {
        _currClipIndex = playerSFX[i];
        playerAudioSource.clip = _currClipIndex;
        playerAudioSource.Play();
    }
}
