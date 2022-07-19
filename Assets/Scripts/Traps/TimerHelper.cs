using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerHelper : MonoBehaviour
{
    public float minTime = 3;
    public float maxTime = 7;
    public float randomTime;

    private void Awake()
    {
        GetRandomTime();

    }

    public void GetRandomTime()
    {
        randomTime = Random.Range(minTime, maxTime);
        //TurnToGetRandom();
    }

    public void TurnToGetRandom()
    {
        Invoke(nameof(GetRandomTime), randomTime);
    }
}
