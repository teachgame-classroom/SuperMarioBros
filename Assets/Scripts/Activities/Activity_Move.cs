using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity_Move : Activity
{
    protected Activity_Input input;
    protected Activity_UpdateAnim updateAnim;


    private float movePower;
    private Rigidbody2D body;

    private Vector2 inputAxis = new Vector2(0,0);

    public Activity_Move() : base() { }

    public Activity_Move(Actor owner, float movePower) : base(owner)
    {
        SetOwner(owner);
    }

    public override void Update()
    {
        Vector3 force = Vector3.right * inputAxis.x;

        if (Time.timeScale > 0)
        {
            body.AddForce(force * movePower);
        }

        float speed = body.velocity.magnitude;

        updateAnim.speed = speed;
        updateAnim.isBreaking = ShouldBreak(speed, inputAxis.x);
    }

    public override void SetOwner(Actor owner)
    {
        base.SetOwner(owner);
        this.movePower = ((Mario)owner).movePower;
        body = owner.GetComponent<Rigidbody2D>();
        input = container.Get<Activity_Input>();
        input.onAxis += OnAxis;
        updateAnim = container.Get<Activity_UpdateAnim>();
    }

    bool ShouldBreak(float speed, float accerlation)
    {
        if (speed > 1 && accerlation < 0 || speed < -1 && accerlation > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void OnAxis(float h, float v)
    {
        inputAxis.Set(h, v);
    }
}
