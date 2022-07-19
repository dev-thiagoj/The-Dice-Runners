using System.Collections.Generic;
using UnityEngine;
using Singleton;

public class SoundManager : Singleton<SoundManager>
{
    public List<MusicSetup> musicSetups;
    public List<SFXSetup> sfxSetups;

    [Header("Sound On/Off")]
    
    public AudioSource musicSource;

    protected override void Awake()
    {
        base.Awake();

        
    }

    public void PlayMusicbyType(MusicType musicType)
    {
        var music = GetMusicByType(musicType);

        musicSource.clip = music.audioClip;
        musicSource.Play();
    }

    public MusicSetup GetMusicByType(MusicType musicType)
    {
        return musicSetups.Find(i => i.musicType == musicType);
    }
    
    public SFXSetup GetSFXByType(SFXType sfxType)
    {
        return sfxSetups.Find(i => i.sfxType == sfxType);
    }

    public void TurnMusicOff()
    {
        musicSource.enabled = false;
        musicSource.Pause();
    }

    public void TurnMusicOn()
    {
        musicSource.enabled = true;
        musicSource.Play();
    }
}

public enum MusicType
{
    NONE,
    AMBIENCE_MAIN,
    LEVEL_WIN,
    LEVEL_LOSE,
}

[System.Serializable]
public class MusicSetup
{
    public MusicType musicType;
    public AudioClip audioClip;
}

public enum SFXType
{
    NONE,
    DICE_COLLECT,
    TURBO_COLLECT,
    FOOTSTEPS
}

[System.Serializable]
public class SFXSetup
{
    public SFXType sfxType;
    public AudioClip audioClip;
}
