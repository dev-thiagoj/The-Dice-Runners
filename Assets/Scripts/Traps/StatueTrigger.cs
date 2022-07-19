using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueTrigger : MonoBehaviour
{
    public StatueDManager statueD;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            statueD.PutGravity();
        }
    }
}
