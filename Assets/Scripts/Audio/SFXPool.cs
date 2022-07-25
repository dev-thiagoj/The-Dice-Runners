using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;

public class SFXPool : Singleton<SFXPool>
{
    public int poolSize = 10;
    public float poolVolume = 1;
    private List<AudioSource> _audioSourcesList;
    private int _index = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    public void CreatePool()
    {
        _audioSourcesList = new List<AudioSource>();
        
        for(int i = 0; i < poolSize; i++)
        {
            CreateAudioSourceItem();
            _audioSourcesList[i].volume = poolVolume;
        }
    }

    private void CreateAudioSourceItem()
    {
        GameObject go = new GameObject("SFX_Pool");
        go.transform.SetParent(gameObject.transform);
        _audioSourcesList.Add(go.AddComponent<AudioSource>());
    }

    public void Play(SFXType sfxType)
    {
        if (sfxType == SFXType.NONE_00) return;

        var sfx = SFXManager.Instance.GetSFXByType(sfxType);

        _audioSourcesList[_index].clip = sfx.audioClip;
        _audioSourcesList[_index].Play();

        _index++;

        if (_index >= _audioSourcesList.Count) _index = 0;
    }
}
