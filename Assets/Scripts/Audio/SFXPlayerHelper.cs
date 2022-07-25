using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayerHelper : MonoBehaviour
{
    //public SFXType sfxType;
    public AudioSource audioSource;

    public List<AudioClip> audioClipsList;
    private AudioClip _currAudioIndex;

    [Header("Delays")]
    public float minDelay = 3f;
    public float maxDelay = 10f;

    private void OnValidate()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (audioSource == null) return;

        _currAudioIndex = audioClipsList[0];
        audioSource.clip = _currAudioIndex;

        StartCoroutine(PlayAudioCoroutine());
    }

    public void PlayFootstepsSFX()
    {
        SFXPool.Instance.Play(SFXType.FOOTSTEPS_01);
    }

    public void PlayAudioSource()
    {
        audioSource.Play();
    }
    IEnumerator PlayAudioCoroutine()
    {
        while (true)
        {
            _currAudioIndex = audioClipsList[0];
            audioSource.clip = _currAudioIndex;
            PlayAudioSource();
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }

    [NaughtyAttributes.Button]
    public void PlayEnemyDead()
    {
        StopAllCoroutines();
        _currAudioIndex = audioClipsList[1];
        audioSource.clip = _currAudioIndex;
        audioSource.Play();
    }
}
