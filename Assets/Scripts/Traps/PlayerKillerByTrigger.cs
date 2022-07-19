using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillerByTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (PlayerController.Instance._isAlive == true) PlayerController.Instance.Dead();
        }
    }
}