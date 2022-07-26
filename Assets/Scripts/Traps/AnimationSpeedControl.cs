using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeedControl : MonoBehaviour
{
    [SerializeField] Animator animator;

    [Range(0,1)]
    public float animSpeed = 1;

    private void Start()
    {
        
    }

    private void Update()
    {
        SetAnimSpeed();
    }

    public void SetAnimSpeed()
    {
        animator.speed = animSpeed;
    }
}
