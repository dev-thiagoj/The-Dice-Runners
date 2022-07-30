using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLookAt : MonoBehaviour
{
    public bool canLook;
    public Transform target;
    

    void Update()
    {
        if (canLook)
        {
            var lookPos = target.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
        }
    }

    public Transform FindTarget()
    {
       var targetTransf = GameObject.Find("PFB_Piece_EndGame/EndGame/FemaleCharacter").GetComponent<Transform>();

        target = targetTransf;

        return target;
    }
}
