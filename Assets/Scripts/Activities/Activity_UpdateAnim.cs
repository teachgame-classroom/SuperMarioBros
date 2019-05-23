using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTools;

public class Activity_UpdateAnim : Activity
{
    public float speed;
    public bool isOnGround;
    public bool isBreaking;

    private RuntimeAnimatorController[] marioControllers;

    protected Animator anim;

    public Activity_UpdateAnim() : base()
    {
    }

    public override void SetOwner(Actor owner)
    {
        base.SetOwner(owner);
        anim = owner.GetComponent<Animator>();
        marioControllers = ((Mario)owner).marioControllers;
        EventManager.RegisterEvent<int>("MarioChangeState", OnMarioChangeState);
        EventManager.RegisterEvent("MarioShoot", OnMarioShoot);
    }
    

    public override void Update()
    {
        anim.SetFloat("speed", speed);
        anim.SetBool("grounded", isOnGround);
        anim.SetBool("break", isBreaking);
    }

    void OnMarioShoot()
    {
        anim.runtimeAnimatorController = marioControllers[3];
        Scheduler.instance.Schedule(0.1f, false, ResetFireAnimatorController);
    }

    void OnMarioChangeState(int newState)
    {
        anim.runtimeAnimatorController = marioControllers[newState];
    }

    private void ResetFireAnimatorController()
    {
        anim.runtimeAnimatorController = marioControllers[AppConst.MARIO_FIRE];
    }
}
