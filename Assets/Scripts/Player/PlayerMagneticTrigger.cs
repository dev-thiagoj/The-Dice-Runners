using UnityEngine;

public class PlayerMagneticTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ItemCollectableBase i = other.transform.GetComponent<ItemCollectableBase>();

        if(i != null)
        {
            i.gameObject.AddComponent<Magnetic>();
        }
    }
}
