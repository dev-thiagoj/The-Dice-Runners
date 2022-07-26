using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            GameManager.Instance.checkedEndLine = true;
            PlayerController.Instance.isInvencible = true;
            GameManager.Instance.EndGame();
        }
    }
}
