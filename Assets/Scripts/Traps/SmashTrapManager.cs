using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SmashTrapManager : MonoBehaviour
{
    public Transform trap;
    public ParticleSystem particleSystem;
    public Collider collider;
    public GameObject smashCollider;
    public MeshRenderer meshRenderer;

    [Space]
    public float dir;
    public float moveDuration;
    public float delaySmashs;
    public float timeToDestroy = 3;
    public Ease ease;

    private void OnValidate()
    {
        //if (_timerHelper == null) _timerHelper = GetComponentInParent<TimerHelper>();
        if (trap == null) trap = GetComponent<Transform>();
        if (particleSystem == null) particleSystem = GetComponentInChildren<ParticleSystem>();
        if (collider == null) collider = GetComponent<Collider>();
        if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
    }
    
    public void DisableSmash()
    {
        smashCollider.SetActive(false);
    }

    public void EnableSmash()
    {
        smashCollider.SetActive(true);
    }

    void Start()
    {
        Invoke(nameof(StartCoroutine), 2);
    }

    public void StartCoroutine()
    {
        StartCoroutine(SmashTrapCoroutine());
    }

    public void TrapClose()
    {
        transform.DOMoveX(1 * dir, moveDuration).SetLoops(2, LoopType.Yoyo).SetEase(ease);
        Invoke(nameof(DisableSmash), 1);
    }

    public IEnumerator SmashTrapCoroutine()
    {
        while (true)
        {
            EnableSmash();
            TrapClose();
            yield return new WaitForSeconds(delaySmashs);
            
        }
    }

    /*public void HideTrap()
    {
        meshRenderer.enabled = false;
        collider.enabled = false;
        Invoke(nameof(DestroyTrap), timeToDestroy);
    }

    public void DestroyTrap()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Dice"))
        {
            HideTrap();
            //particleSystem.Play();
        }
            
    }*/
}
