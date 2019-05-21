using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity_Move : Activity
{
    protected Activity_Input input;
    private float movePower;
    private Rigidbody2D body;

    private Vector2 inputAxis = new Vector2(0,0);

    public Activity_Move() : base() { }

    public Activity_Move(Actor owner, float movePower) : base(owner)
    {
        body = owner.GetComponent<Rigidbody2D>();
        this.movePower = movePower;
        input = (Activity_Input)owner.activities[typeof(Activity_Input)];
        input.onAxis += OnAxis;
    }

    public override void Update()
    {
        Vector3 force = Vector3.right * inputAxis.x;

        if (Time.timeScale > 0)
        {
            body.AddForce(force * movePower);
        }
    }

    public override void SetOwner(Actor owner)
    {
        base.SetOwner(owner);
        this.movePower = ((Mario)owner).movePower;

        body = owner.GetComponent<Rigidbody2D>();
        input = container.Get<Activity_Input>();
        input.onAxis += OnAxis;
    }

    private void OnAxis(float h, float v)
    {
        inputAxis.Set(h, v);
    }
}
