using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsSFX : MonoBehaviour
{
    public Animator animator;

    public void PlayFootsteps()
    {
        SFXPool.Instance.Play(SFXType.FOOTSTEPS_01);
    }
}
