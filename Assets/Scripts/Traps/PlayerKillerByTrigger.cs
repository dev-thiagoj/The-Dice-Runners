using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillerByTrigger : MonoBehaviour
{
    public AudioSource audioSource;

    [Range(0, 1)]
    public float sfxVolume;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (PlayerController.Instance._isAlive == true) PlayerController.Instance.Dead();
            PlaySFX();
        }
    }

    public void PlaySFX()
    {
        //SFXPool.Instance.Play(sfxType);
        audioSource.volume = sfxVolume;
        audioSource.Play();
    }
}