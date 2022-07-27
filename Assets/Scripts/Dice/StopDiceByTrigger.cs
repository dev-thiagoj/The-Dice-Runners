using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopDiceByTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Dice"))
        {
            RollDice.Instance.canMove = false;
            RollDice.Instance.audioSource.Stop();
        }
    }
}
