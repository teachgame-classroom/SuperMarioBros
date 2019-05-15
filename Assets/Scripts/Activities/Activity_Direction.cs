using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity_Direction : Activity
{
    private Activity_Input input;
    private Transform transform;

    public Activity_Direction(Actor owner): base(owner)
    {
        transform = owner.transform;
        input = (Activity_Input)owner.activities[typeof(Activity_Input)];
    }

    public override void Update()
    {
        float h = input.h;

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
