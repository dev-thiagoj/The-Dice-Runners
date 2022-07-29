using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using System;

public class RollDice : Singleton<RollDice>
{
    public Rigidbody rigidbody;
    public Transform diceToRoll;
    public AudioSource audioSource;

    [Range(4, 6)]
    public float speedRoll = 3;
    public float moveSpeed = 5;
    public float startSFXDelay = 3;
    public bool canMove;

    protected override void Awake()
    {
        base.Awake();
    }

    #region === DEBUG ===
    public void TurnCanRollTrue()
    {
        canMove = true;
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            rigidbody.transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
            
            diceToRoll.transform.Rotate(0.0f, -speedRoll, 0.0f);
        }
    }

    public void InvokeStartRoll()
    {
        Invoke(nameof(StartRoll), 3);
    }

    public void StartRoll()
    {
        canMove = true;
        Actions.startTutorial();
    }

    public void DestroyDice()
    {
        Destroy(gameObject, 5);
    }

    public void CallDiceSFX()
    {
        Invoke(nameof(PlaySFX), startSFXDelay);
    }

    public void PlaySFX()
    {
        audioSource.Play();
    }
}
