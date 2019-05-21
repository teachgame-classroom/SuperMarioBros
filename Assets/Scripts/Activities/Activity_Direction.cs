using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity_Direction : Activity
{
    private Activity_Input input;
    private Transform transform;
    private Vector2 inputAxis;

    public Activity_Direction() : base() { }

    public Activity_Direction(Actor owner): base(owner)
    {
        transform = owner.transform;
        input = (Activity_Input)owner.activities[typeof(Activity_Input)];
        input.onAxis += OnAxis;
    }

    public override void SetOwner(Actor owner)
    {
        base.SetOwner(owner);
        transform = owner.transform;
        input = container.Get<Activity_Input>();
        input.onAxis += OnAxis;

    }

    public override void Update()
    {
        if (inputAxis.x > 0)
        {
            transform.rotation = Quaternion.identity;
        }
        else if (inputAxis.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void OnAxis(float h, float v)
    {
        inputAxis.Set(h, v);
    }

}
