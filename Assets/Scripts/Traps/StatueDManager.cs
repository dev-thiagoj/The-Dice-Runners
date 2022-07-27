using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueDManager : MonoBehaviour
{
    public Rigidbody rigidbody;
    public AudioSource audioHitFloor;
    public AudioSource audioHitPlayer;
    public List<AudioClip> audioClips;


    public void PutGravity()
    {
        rigidbody.useGravity = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerController.Instance.Dead();
            audioHitPlayer.Play();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("StatueTrigger"))
        {   
            audioHitFloor.Play();
        }
    }
}
