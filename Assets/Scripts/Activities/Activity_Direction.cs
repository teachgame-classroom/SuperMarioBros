using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity_Direction : Activity
{
    private Transform transform;

    public Activity_Direction(MonoBehaviour owner)
    {
        transform = owner.transform;
    }

    public override void Update()
    {
        float h = Input.GetAxis("Horizontal");

        if (h > 0)
        {
            transform.rotation = Quaternion.identity;
        }
        else if (h < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
