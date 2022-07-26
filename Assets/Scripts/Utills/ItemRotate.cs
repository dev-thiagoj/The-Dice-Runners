using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    public float speed;
    public float minSpeed = 3;
    public float maxSpeed = 5;
    private Vector3 _axis;

    public bool isSpeedRandom;
    public bool rotateX;
    public bool rotateY;
    public bool rotateZ;

    private void Start()
    {
        if (rotateX) _axis = Vector3.right;
        else if (rotateY) _axis = Vector3.up;
        else if (rotateZ) _axis = Vector3.forward;

        if (isSpeedRandom) speed = Random.Range(minSpeed, maxInclusive: maxSpeed);
    }

    void Update()
    {
        transform.Rotate(_axis * speed);
    }
}
