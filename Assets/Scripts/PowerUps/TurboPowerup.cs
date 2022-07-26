using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurboPowerup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            PlayerController.Instance._currTurbo--;
        }
    }
}
