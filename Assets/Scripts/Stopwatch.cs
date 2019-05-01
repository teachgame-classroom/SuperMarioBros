using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stopwatch
{
    public bool isCounting;

    public float startTime;
    public float endTime;

    public float maxTime;

    public float normalizedTime { get { return Mathf.Clamp01(time / maxTime); } }
    public float time;

    public Stopwatch(float maxTime = 1)
    {
        this.maxTime = maxTime;
    }

    public void Start()
    {
        startTime = Time.realtimeSinceStartup;
        endTime = startTime;
        isCounting = true;
    }

    public float Stop()
    {
        endTime = Time.realtimeSinceStartup;
        time = endTime - startTime;
        isCounting = false;
        Debug.Log("Normalized:" + normalizedTime);
        return time;
    }

    public float TimeSinceStart()
    {
        if(isCounting)
        {
            return Time.realtimeSinceStartup - startTime;
        }
        else
        {
            return 0;
        }
    }
}
