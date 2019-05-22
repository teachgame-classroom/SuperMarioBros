using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity_UpdateAnim : Activity
{
    public float speed;
    public bool isOnGround;
    public bool isBreaking;

    protected Animator anim;

    public Activity_UpdateAnim() : base()
    {
    }

    public override void SetOwner(Actor owner)
    {
        base.SetOwner(owner);
        anim = owner.GetComponent<Animator>();
    }

    public override void Update()
    {
        anim.SetFloat("speed", speed);
        anim.SetBool("grounded", isOnGround);
        anim.SetBool("break", isBreaking);
    }

}
