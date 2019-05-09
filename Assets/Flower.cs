using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    private Vector3 originPos;
    private Vector3 targetPos;

    private float t;

    // Update is called once per frame
    void Start()
    {
        originPos = transform.position;
        targetPos = originPos + Vector3.up;
    }

    void Update()
    {
        t += Time.deltaTime;
        transform.position = Vector3.Lerp(originPos, targetPos, t);
    }
}
