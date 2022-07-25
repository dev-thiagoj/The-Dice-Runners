using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldPlayerPosition : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Wall"))
        {
            var posZ = transform.position.z;

            PlayerController.Instance.transform.position = new Vector3(transform.position.x, transform.position.y, posZ);
        }
    }
}
