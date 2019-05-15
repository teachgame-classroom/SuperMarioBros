using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity_StateManagement : Activity
{
    private Activity_Input input;

    public int state { get; private set; }

    private Rigidbody2D body;
    private Collider2D col;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private RuntimeAnimatorController[] marioControllers;

    private float changeStateBeginTime;
    private Sprite oldSprite;

    private int spriteIndex;

    private bool isChangingState;

    private bool invincible;
    private float invincibleBeginTime;

    private float changeStatePauseTime = 1f;
    private float blinkInterval = 0.05f;

    private float lastBlinkTime;

    private AudioClip pipeClip;
    private AudioClip powerupClip;
    private AudioClip dieClip;

    private Sprite[] mario_s;
    private Sprite[] mario_b;
    private Sprite[] mario_f;

    public Activity_StateManagement(Actor owner, int state) : base(owner)
    {
        this.state = state;
        input = (Activity_Input)owner.activities[typeof(Activity_Input)];

        body = owner.GetComponent<Rigidbody2D>();
        col = owner.GetComponent<Collider2D>();
        anim = owner.GetComponent<Animator>();
        spriteRenderer = owner.GetComponent<SpriteRenderer>();

        powerupClip = Resources.Load<AudioClip>("Sounds/smb_powerup");
        pipeClip = Resources.Load<AudioClip>("Sounds/smb_pipe");
        dieClip = Resources.Load<AudioClip>("Sounds/smb_mariodie");

        mario_s = ((Mario)owner).mario_s;
        mario_b = ((Mario)owner).mario_b;
        mario_f = ((Mario)owner).mario_f;
        marioControllers = ((Mario)owner).marioControllers;
    }


    public override void Update()
    {
        if (input.test1) BeginChangeState(AppConst.MARIO_SMALL);
        if (input.test2) BeginChangeState(AppConst.MARIO_BIG);
        if (input.test3) BeginChangeState(AppConst.MARIO_FIRE);
        if (input.test4) Die();

        if (isChangingState)
        {
            if (Time.unscaledTime - changeStateBeginTime < changeStatePauseTime)
            {
                if (Time.unscaledTime - lastBlinkTime > blinkInterval)
                {
                    //Debug.Log("Last Blink Time:" + lastBlinkTime);
                    if (spriteRenderer.sprite == oldSprite)
                    {
                        Debug.Log("change to new");
                        ChangeSprite(state, spriteIndex);
                    }
                    else
                    {
                        Debug.Log("change to old");
                        spriteRenderer.sprite = oldSprite;
                    }

                    lastBlinkTime = Time.unscaledTime;
                }

                return;
            }
            else
            {
                ChangeState(state);
            }
        }
    }

    void BeginChangeState(int newState)
    {
        //CancelInvoke("ResetFireAnimatorController");

        anim.enabled = false;

        changeStateBeginTime = Time.unscaledTime;

        oldSprite = spriteRenderer.sprite;
        state = newState;

        string[] spriteNameSplit = oldSprite.name.Split('_');
        string spriteNumberString = spriteNameSplit[spriteNameSplit.Length - 1];
        spriteIndex = int.Parse(spriteNumberString);

        Debug.Log("Sprite编号：" + spriteIndex);

        isChangingState = true;

        if (newState == AppConst.MARIO_SMALL)
        {
            int playerMask = LayerMask.NameToLayer("Player");
            Physics2D.SetLayerCollisionMask(playerMask, LayerMask.GetMask(new string[] { "Stage" }));

            invincible = true;
            invincibleBeginTime = Time.unscaledTime;
            AudioSource.PlayClipAtPoint(pipeClip, Camera.main.transform.position);
        }
        else
        {
            AudioSource.PlayClipAtPoint(powerupClip, Camera.main.transform.position);
            Debug.Log("Powerup");
        }

        Time.timeScale = 0;
    }

    void ChangeState(int newState)
    {
        Time.timeScale = 1;
        isChangingState = false;

        anim.enabled = true;

        float height = 1;

        switch (newState)
        {
            case AppConst.MARIO_SMALL:
                height = 1;
                break;
            case AppConst.MARIO_BIG:
                height = 2;
                break;
            case AppConst.MARIO_FIRE:
                height = 2;
                break;
            default:
                break;
        }

        spriteRenderer.color = Color.white;

        (col as BoxCollider2D).size = new Vector2((col as BoxCollider2D).size.x, height);
        (col as BoxCollider2D).offset = new Vector2(0, height / 2);

        anim.runtimeAnimatorController = marioControllers[newState];

        Treasure.OnMarioStateChange(newState);
    }

    void ChangeSprite(int state, int index)
    {
        Sprite[] sprites = mario_s;

        if (state == AppConst.MARIO_BIG)
        {
            Debug.Log("BIG");
            sprites = mario_b;
        }

        if (state == AppConst.MARIO_FIRE)
        {
            Debug.Log("FIRE");
            sprites = mario_f;
        }

        spriteRenderer.color = new Color(1, 1, 1, 0.75f);
        spriteRenderer.sprite = sprites[index];
        Debug.Log("new sprite:" + spriteRenderer.sprite.name);
    }

    public void Die()
    {
        state = AppConst.MARIO_DIE;
        body.velocity = Vector3.zero;
        body.isKinematic = true;
        anim.SetBool("dead", true);
        Camera.main.GetComponent<AudioSource>().clip = dieClip;
        Camera.main.GetComponent<AudioSource>().loop = false;
        Camera.main.GetComponent<AudioSource>().Play();
        //Invoke("DieFall", 0.5f);
    }
}
