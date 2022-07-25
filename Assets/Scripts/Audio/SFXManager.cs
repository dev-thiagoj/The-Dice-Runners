using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;

public class SFXManager : Singleton<SFXManager>
{
    public List<SFXSetup> sfxSetups;

    protected override void Awake()
    {
        base.Awake();
    }

    public SFXSetup GetSFXByType(SFXType sfxType)
    {
        return sfxSetups.Find(i => i.sfxType == sfxType);
    }
}

public enum SFXType
{
    NONE_00,
    FOOTSTEPS_01,
    JUMP_02,
    DEATH_03,
    DICE_COLLECT_04,
    TURBO_COLLECT_05,
    USE_TURBO_06,
    MAGNETIC_COLLECT_07,
    USE_MAGNETIC_08,
    TRAP_DESTROY_09
}

[System.Serializable]
public class SFXSetup
{
    public SFXType sfxType;
    public AudioClip audioClip;
}
