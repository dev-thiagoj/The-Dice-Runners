using UnityEngine;
using Singleton;

public class MusicPlayer : Singleton<MusicPlayer>
{
    public MusicType musicAmbience;
    public AudioSource audioSource;

    private MusicSetup _currentMusicSetup;

    private void Start()
    {
        PlayAmbience();
    }

    private void PlayAmbience()
    {
        _currentMusicSetup = SoundManager.Instance.GetMusicByType(MusicType.AMBIENCE_MAIN);

        audioSource.clip = _currentMusicSetup.audioClip;
        audioSource.Play();
    }

    public void PlayWinJingle()
    {
        _currentMusicSetup = SoundManager.Instance.GetMusicByType(MusicType.LEVEL_WIN);

        audioSource.clip = _currentMusicSetup.audioClip;
        audioSource.Play();

        Invoke(nameof(PlayAmbience), 10);
    }

    public void PlayLoseJingle()
    {
        _currentMusicSetup = SoundManager.Instance.GetMusicByType(MusicType.LEVEL_LOSE);

        audioSource.clip = _currentMusicSetup.audioClip;
        audioSource.Play();

        Invoke(nameof(PlayAmbience), 6);
    }
}
