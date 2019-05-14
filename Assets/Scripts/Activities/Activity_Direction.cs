using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity_Direction : Activity
{
    private Transform transform;

    public Activity_Direction(Actor owner) : base(owner)
    {
        transform = owner.transform;
    }

    protected override bool Evaluate()
    {
        return true;
    }

    protected override void DoActivity()
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
