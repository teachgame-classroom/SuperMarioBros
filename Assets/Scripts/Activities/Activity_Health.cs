using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTools;

public class Activity_Health : Activity
{
    public int maxHp;
    public int currentHp;

    private bool invincible;
    private float invincibleBeginTime;
    private float invincibleTime = 5f;

    protected Activity_StateManagement stateManagement;
    protected Activity_Raycast raycast;

    private float blinkInterval = 0.05f;
    private int InvincibleTimesUpHandler;
    private int blinkTimerHandler;

    private SpriteRenderer spriteRenderer;

    public Activity_Health()
    {
    }

    public override void SetOwner(Actor owner)
    {
        Debug.Log("Health SetOwner");
        base.SetOwner(owner);
        stateManagement = container.Get<Activity_StateManagement>();
        raycast = container.Get<Activity_Raycast>();
        spriteRenderer = owner.GetComponent<SpriteRenderer>();

        EventManager.RegisterEvent("RaycastHurt", Hurt);
        EventManager.RegisterEvent<int>("MarioChangeState", OnMarioChangeState);
    }

    public override void Update()
    {
        
    }

    protected void Hurt()
    {
        if (invincible || stateManagement.state == AppConst.MARIO_DIE) return;
        
        EventManager.ExecuteEvent("MarioHurt");
    }

    void OnMarioChangeState(int newState)
    {
        if(newState == AppConst.MARIO_SMALL)
        {
            BeginInvincible();
        }
    }

    private void BeginInvincible()
    {
        invincible = true;

        int playerMask = LayerMask.NameToLayer("Player");
        Physics2D.SetLayerCollisionMask(playerMask, LayerMask.GetMask(new string[] { "Stage" }));

        InvincibleTimesUpHandler = Scheduler.instance.Schedule(invincibleTime, false, OnTimesUp_Invincible);
        blinkTimerHandler = Scheduler.instance.Schedule(blinkInterval, true, OnTimesUp_ChangeStateBlink);
    }

    void OnTimesUp_Invincible()
    {
        Scheduler.instance.Deschedule(blinkTimerHandler);

        invincible = false;
        Scheduler.instance.Schedule(0.1f, false, () => { spriteRenderer.color = Color.white; });
        Debug.Log("Reset:" + Time.unscaledTime);
        int playerMask = LayerMask.NameToLayer("Player");
        Physics2D.SetLayerCollisionMask(playerMask, LayerMask.GetMask(new string[] { "Stage", "Enemy" }));
    }

    void OnTimesUp_ChangeStateBlink()
    {
        if (spriteRenderer.color == Color.white)
        {
            Debug.Log("Change Clean");
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            Debug.Log("Change White");
            spriteRenderer.color = Color.white;
        }
        Debug.Log("Blink:" + Time.unscaledTime);
    }
}
