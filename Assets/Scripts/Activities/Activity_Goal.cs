using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTools;

public class Activity_Goal : Activity
{
    protected bool hasReachedGoal;
    protected Vector3 flagBottomPos;

    protected Animator anim;
    protected Rigidbody2D body;
    protected bool goingToCastle;

    protected Activity_StateManagement stateManagement;
    protected Activity_Move move;

    public Activity_Goal() : base()
    {
    }

    public override void SetOwner(Actor owner)
    {
        base.SetOwner(owner);
        stateManagement = container.Get<Activity_StateManagement>();
        move = container.Get<Activity_Move>();

        anim = owner.GetComponent<Animator>();
        body = owner.GetComponent<Rigidbody2D>();

        EventManager.RegisterEvent<Collider2D>("MarioTriggerEnter", OnMarioTriggerEnter);
    }

    public override void Update()
    {
        if (stateManagement.state != AppConst.MARIO_DIE)
        {
            // 到达旗帜
            if (stateManagement.hasReachedGoal)
            {
                // 旗帜落下前
                if (!goingToCastle)
                {
                    // 自动滑下旗杆
                    owner.transform.position = Vector3.MoveTowards(owner.transform.position, flagBottomPos, Time.deltaTime * 6);
                }
                else
                {
                    // 自动前进到城堡
                    body.AddForce(Vector3.right * move.movePower);
                }
            }
        }
    }

    void OnMarioTriggerEnter(Collider2D collider)
    {
        if (collider.tag == "Goal")
        {
            Goal(collider.gameObject);
        }

        if (collider.tag == "Gate")
        {
            StageClear();
        }
    }

    void Goal(GameObject goal)
    {
        if (stateManagement.hasReachedGoal)
        {
            return;
        }
        else
        {
            ChangeBGM("Sounds/smb_flagpole", false);

            anim.SetBool("hug", true);
            anim.SetFloat("speed", 0);
            owner.transform.position = Vector3.right * (goal.transform.position.x + 0.2f) + Vector3.up * owner.transform.position.y;

            flagBottomPos = Vector3.right * (goal.transform.position.x + 0.2f) + Vector3.up * 3;

            body.velocity = Vector3.zero;
            body.isKinematic = true;

            goal.GetComponentInParent<Animator>().SetTrigger("down");

            Scheduler.instance.Schedule(1.55f, false, BeginGotoCastle);

            EventManager.ExecuteEvent("MarioReachGoal");
        }
    }

    private void BeginGotoCastle()
    {
        owner.transform.position = Vector3.right * (int)(owner.transform.position.x + 1) + Vector3.up * 2;
        body.isKinematic = false;
        body.gravityScale = 1;
        anim.SetBool("hug", false);
        anim.SetFloat("speed", move.movePower);
        anim.SetBool("grounded", true);
        goingToCastle = true;

        ChangeBGM("Sounds/smb_stage_clear", false);
    }


    void StageClear()
    {
        owner.gameObject.SetActive(false);
    }

    private void ChangeBGM(string bgmPath, bool loop)
    {
        AudioSource bgm = Camera.main.GetComponent<AudioSource>();
        bgm.clip = Resources.Load<AudioClip>(bgmPath);
        bgm.loop = loop;
        bgm.Play();
    }



}
