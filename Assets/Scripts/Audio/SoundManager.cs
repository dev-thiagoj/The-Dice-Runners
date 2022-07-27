using System.Collections.Generic;
using UnityEngine;
using Singleton;

public class SoundManager : Singleton<SoundManager>
{
    public AudioListener mainListener;
    public GameObject[] soundButtons;
    int _index;
    bool _check;

    //Not in use

    //public List<MusicSetup> musicSetups;
    //public AudioSource musicSource;


    protected override void Awake()
    {
        base.Awake();

        _check = true;
        _index = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _check = !_check;
            TurnSoundOnOff(_check);
            TurnImageOnOff();
        }
    }

    public void TurnSoundOnOff(bool b)
    {
        mainListener.enabled = b;
    }

    public void TurnImageOnOff()
    {
        foreach (var button in soundButtons)
        {
            button.SetActive(false);
        }

        soundButtons[_index].SetActive(true);
        _index++;

        if (_index == 2) _index = 0;
    }

    #region === NotInUse ===

    /*public void PlayMusicbyType(MusicType musicType)
    {
        var music = GetMusicByType(musicType);

        musicSource.clip = music.audioClip;
        musicSource.Play();
    }

    public MusicSetup GetMusicByType(MusicType musicType)
    {
        return musicSetups.Find(i => i.musicType == musicType);
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
    }*/
    #endregion
}

/*public enum MusicType
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
}*/