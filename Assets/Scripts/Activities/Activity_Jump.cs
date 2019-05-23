using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity_Jump : Activity
{
    protected float jumpMaxPower;
    protected bool isJumpingUp;

    protected Activity_Raycast raycast;
    protected Activity_Input input;
    protected Rigidbody2D body;

    protected AudioClip jumpClip;

    public Activity_Jump() : base(){ }

    public override void SetOwner(Actor owner)
    {
        base.SetOwner(owner);
        input = container.Get<Activity_Input>();
        raycast = container.Get<Activity_Raycast>();
        body = owner.GetComponent<Rigidbody2D>();
        jumpMaxPower = ((Mario)owner).jumpMaxPower;
        input.onButtonDown_Jump += Jump;
        input.onButtonUp_Jump += Dive;

        jumpClip = Resources.Load<AudioClip>("Sounds/smb_jump-small");
    }

    public override void Update()
    {
        if (body.velocity.y < 0)
        {
            isJumpingUp = false;
        }
    }

    void Jump()
    {
        if (raycast.isOnGround)
        {
            body.AddForce(Vector3.up * jumpMaxPower, ForceMode2D.Impulse);
            isJumpingUp = true;
            AudioSource.PlayClipAtPoint(jumpClip, Camera.main.transform.position);
        }
    }

    void Dive()
    {
        if (isJumpingUp)
        {
            Debug.Log("Dive");
            isJumpingUp = false;
            body.velocity = new Vector2(body.velocity.x, Mathf.Min(body.velocity.y, 10));
        }
    }
}
