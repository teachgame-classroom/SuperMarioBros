using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity_Move : Activity
{
    private float movePower = 10;
    private Rigidbody2D body;

    public Activity_Move(Actor owner): base(owner)
    {
        body = owner.GetComponent<Rigidbody2D>();
//        this.movePower = movePower;
    }

    protected override bool Evaluate()
    {
        return true;
    }

    protected override void DoActivity()
    {
        float h = Input.GetAxis("Horizontal");

        Vector3 force = Vector3.right * h;

        if (Time.timeScale > 0)
        {
            body.AddForce(force * movePower);
        }
    }
}
