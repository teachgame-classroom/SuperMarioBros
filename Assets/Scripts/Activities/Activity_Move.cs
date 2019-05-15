using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity_Move : Activity
{
    protected Activity_Input input;
    private float movePower;
    private Rigidbody2D body;

    public Activity_Move(Actor owner, float movePower) : base(owner)
    {
        body = owner.GetComponent<Rigidbody2D>();
        this.movePower = movePower;
        input = (Activity_Input)owner.activities[typeof(Activity_Input)];
    }

    public override void Update()
    {
        float h = input.h;

        Vector3 force = Vector3.right * h;

        if (Time.timeScale > 0)
        {
            body.AddForce(force * movePower);
        }
    }
}
