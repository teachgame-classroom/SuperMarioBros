using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : MonoBehaviour
{
    public const int MARIO_DIE = -1;
    public const int MARIO_SMALL = 0;
    public const int MARIO_BIG = 1;
    public const int MARIO_FIRE = 2;

    public int state = MARIO_SMALL;

    [Range(0,20)]
    public float movePower = 10;

    [Range(0, 20)]
    public float jumpMinPower = 20;

    [Range(20, 30)]
    public float jumpMaxPower = 25;

    private bool invincible;
    private bool isBreaking;
    private bool isOnGround;
    private bool isJumpingUp;
    private bool isChangingState;
    private bool hasReachedGoal;
    private bool goingToCastle;

    private Vector3 flagBottomPos;

    private float invincibleTime = 5f;
    private float invincibleBeginTime;
    private float changeStateBeginTime;
    private float changeStatePauseTime = 1f;
    private float blinkInterval = 0.05f;
    private float lastBlinkTime;

    private Rigidbody2D body;
    private Collider2D col;

    private Transform shotPos;
    private GameObject bulletPrefab;

    public Sprite[] mario_s;
    public Sprite[] mario_b;
    public Sprite[] mario_f;

    public RuntimeAnimatorController[] marioControllers;
    private Animator anim;

    private int spriteIndex;
    private Sprite oldSprite;
    private SpriteRenderer spriteRenderer;

    private AudioClip powerupClip;
    private AudioClip pipeClip;
    private AudioClip dieClip;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        anim = GetComponent<Animator>();

        shotPos = transform.Find("ShotPos");
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Mario_Bullet");

        powerupClip = Resources.Load<AudioClip>("Sounds/smb_powerup");
        pipeClip = Resources.Load<AudioClip>("Sounds/smb_pipe");
        dieClip = Resources.Load<AudioClip>("Sounds/smb_mariodie");
    }

    // Update is called once per frame
    void Update()
    {
        if(state != MARIO_DIE)
        {
            if (hasReachedGoal)
            {
                if(!goingToCastle)
                {
                    transform.position = Vector3.MoveTowards(transform.position, flagBottomPos, Time.deltaTime * 6);
                }
                else
                {
                    body.AddForce(Vector3.right * movePower);
                }
            }
            else
            {
                if (!isChangingState)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        BeginChangeState(MARIO_SMALL);
                    }

                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        BeginChangeState(MARIO_BIG);
                    }

                    if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        BeginChangeState(MARIO_FIRE);
                    }
                }

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

                if (invincible)
                {
                    Debug.Log("Invincible");
                    if (Time.unscaledTime - invincibleBeginTime < invincibleTime)
                    {
                        Debug.Log("Time.time:" + Time.time + ",lastBlinkTime:" + lastBlinkTime);

                        if (Time.unscaledTime - lastBlinkTime > blinkInterval * 3)
                        {
                            Debug.Log("BlinkTime");

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

                            lastBlinkTime = Time.time;
                        }
                    }
                    else
                    {
                        invincible = false;
                        spriteRenderer.color = Color.white;

                        int playerMask = LayerMask.NameToLayer("Player");
                        Physics2D.SetLayerCollisionMask(playerMask, LayerMask.GetMask(new string[] { "Stage", "Enemy" }));
                    }
                }

                bool isOnGround = CheckGroundAndEnemy();
                bool isHurt = CheckHurt();

                if (isHurt)
                {
                    if (state == MARIO_SMALL)
                    {
                        Die();
                    }
                    else
                    {
                        BeginChangeState(MARIO_SMALL);
                    }
                }

                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");
                //Debug.Log("左右方向轴：" + h);
                //Debug.Log("上下方向轴：" + v);

                if (h > 0)
                {
                    transform.rotation = Quaternion.identity;
                }
                else if (h < 0)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }

                Vector3 force = Vector3.right * h;

                if (Time.timeScale > 0)
                {
                    body.AddForce(force * movePower);
                }

                if (isOnGround)
                {
                    float speed = body.velocity.magnitude;

                    anim.SetFloat("speed", speed);

                    if (ShouldBreak(body.velocity.x, h))
                    {
                        if (!isBreaking)
                        {
                            Debug.Log("Break");
                            isBreaking = true;
                            anim.ResetTrigger("break");
                            anim.SetTrigger("break");
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.K))
                    {
                        body.AddForce(Vector3.up * jumpMaxPower, ForceMode2D.Impulse);
                        Debug.Log("Jump Up Speed:" + body.velocity.magnitude);
                        isJumpingUp = true;
                        //jumpTimer.Start();
                    }
                }
                else
                {
                    if (body.velocity.y < 0)
                    {
                        isJumpingUp = false;
                    }

                    if (Input.GetKeyUp(KeyCode.K))
                    {
                        if (isJumpingUp)
                        {
                            isJumpingUp = false;
                            body.velocity = new Vector2(body.velocity.x, Mathf.Min(body.velocity.y, 10));
                        }
                    }
                }

                anim.SetBool("grounded", isOnGround);

                if (state == MARIO_FIRE)
                {
                    if (Input.GetKeyDown(KeyCode.J))
                    {
                        Shoot();
                    }
                }
            }
        }
    }

    private void BeginGotoCastle()
    {
        transform.position = Vector3.right * (int)(transform.position.x + 1) + Vector3.up * 2;
        body.isKinematic = false;
        body.gravityScale = 1;
        anim.SetBool("hug", false);
        anim.SetFloat("speed", movePower);
        anim.SetBool("grounded", true);
        goingToCastle = true;

        ChangeBGM("Sounds/smb_stage_clear", false);
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, shotPos.position, shotPos.rotation);
        anim.runtimeAnimatorController = marioControllers[3];
        Invoke("ResetFireAnimatorController", 0.1f);
    }

    private void ResetFireAnimatorController()
    {
        anim.runtimeAnimatorController = marioControllers[MARIO_FIRE];
    }

    private void Die()
    {
        state = MARIO_DIE;
        body.velocity = Vector3.zero;
        body.isKinematic = true;
        anim.SetBool("dead", true);
        Camera.main.GetComponent<AudioSource>().clip = dieClip;
        Camera.main.GetComponent<AudioSource>().loop = false;
        Camera.main.GetComponent<AudioSource>().Play();
        Invoke("DieFall", 0.5f);
    }

    bool ShouldBreak(float speed, float accerlation)
    {
        if(speed > 1 && accerlation < 0 || speed < -1 && accerlation > 0)
        {
            return true;
        }
        else
        {
            isBreaking = false;
            return false;
        }
    }

    bool CheckGroundAndEnemy()
    {
        body.gravityScale = 5;

        Transform[] hitTrans;
        bool result = ThreeLineCast(col.bounds.center, Vector2.right * col.bounds.extents.x * 1.05f, Vector2.down, col.bounds.extents.y * 1.05f, out hitTrans);

        if(result)
        {
            for(int i = 0; i < hitTrans.Length; i++)
            {
                if(hitTrans[i])
                {
                    Enemy enemy = hitTrans[i].GetComponent<Enemy>();

                    if (enemy)
                    {
                        enemy.OnStomp();
                        Invoke("Rebound", 0.015f);
                    }
                }
            }
            body.gravityScale = 1;
        }

        return result;
    }

    bool ThreeLineCast(Vector3 center, Vector3 offset, Vector3 direction, float distance, out Transform[] hitTrans, bool queriesStartInColliders = false)
    {
        bool originQuerySetting = Physics2D.queriesStartInColliders;

        Physics2D.queriesStartInColliders = queriesStartInColliders;

        hitTrans = new Transform[3];
        hitTrans[0] = Physics2D.Raycast(center, direction, distance).transform;
        hitTrans[1] = Physics2D.Raycast(center + offset, direction, distance).transform;
        hitTrans[2] = Physics2D.Raycast(center - offset, direction, distance).transform;

        Debug.DrawLine(center, center + direction * distance, Color.red);
        Debug.DrawLine(center + offset, center + offset + direction * distance, Color.red);
        Debug.DrawLine(center - offset, center - offset + direction * distance, Color.red);

        Physics2D.queriesStartInColliders = originQuerySetting;

        return hitTrans[0] || hitTrans[1] || hitTrans[2];
    }

    bool CheckHurt()
    {
        if (invincible)
        {
            return false;
        }

        bool rightHurt = CheckHurt(Vector3.up * col.bounds.extents.y * 1.05f, Vector3.right, col.bounds.extents.x * 1.05f);
        bool leftHurt = CheckHurt(Vector3.up * col.bounds.extents.y * 1.05f, Vector3.left, col.bounds.extents.x * 1.05f);
        bool topHurt = CheckHurt(Vector3.right * col.bounds.extents.x * 1.05f, Vector3.up, col.bounds.extents.y * 1.05f);

        return rightHurt || leftHurt || topHurt;
    }

    bool CheckHurt(Vector3 offset, Vector3 direction, float distance)
    {
        Transform[] hitTrans;
        bool hurtResult = false;

        bool lineCastResult = ThreeLineCast(col.bounds.center, offset, direction, distance, out hitTrans);

        if (lineCastResult)
        {
            for (int i = 0; i < hitTrans.Length; i++)
            {
                if (hitTrans[i])
                {
                    Enemy enemy = hitTrans[i].GetComponent<Enemy>();

                    if (enemy)
                    {
                        Debug.Log("Hurt");
                        hurtResult = true;
                        break;
                    }
                }
            }
        }
        return hurtResult;
    }

    void BeginChangeState(int newState)
    {
        CancelInvoke("ResetFireAnimatorController");

        anim.enabled = false;

        changeStateBeginTime = Time.unscaledTime;

        oldSprite = spriteRenderer.sprite;
        state = newState;

        string[] spriteNameSplit = oldSprite.name.Split('_');
        string spriteNumberString = spriteNameSplit[spriteNameSplit.Length - 1];
        spriteIndex = int.Parse(spriteNumberString);

        Debug.Log("Sprite编号：" + spriteIndex);

        isChangingState = true;

        if(newState == MARIO_SMALL)
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

        switch(newState)
        {
            case MARIO_SMALL:
                height = 1;
                break;
            case MARIO_BIG:
                height = 2;
                break;
            case MARIO_FIRE:
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

    public void Pause()
    {

    }

    void ChangeSprite(int state, int index)
    {
        Sprite[] sprites = mario_s;

        if(state == MARIO_BIG)
        {
            Debug.Log("BIG");
            sprites = mario_b;
        }

        if(state == MARIO_FIRE)
        {
            Debug.Log("FIRE");
            sprites = mario_f;
        }

        spriteRenderer.color = new Color(1, 1, 1, 0.75f);
        spriteRenderer.sprite = sprites[index];
        Debug.Log("new sprite:" + spriteRenderer.sprite.name);
    }

    void Rebound()
    {
        body.velocity = Vector2.right * body.velocity.x + Vector2.up * 10;
    }

    void DieFall()
    {
        body.isKinematic = false;
        body.velocity = Vector2.up * 20;
        body.gravityScale = 2.5f;
        GetComponent<Collider2D>().enabled = false;
    }

    private void ChangeBGM(string bgmPath, bool loop)
    {
        AudioSource bgm = Camera.main.GetComponent<AudioSource>();
        bgm.clip = Resources.Load<AudioClip>(bgmPath);
        bgm.loop = loop;
        bgm.Play();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        TryPickupItem(collision.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Goal")
        {
            Goal(collider.gameObject);
        }

        if(collider.tag == "Gate")
        {
            StageClear();
        }
    }

    void Goal(GameObject goal)
    {
        if (hasReachedGoal)
        {
            return;
        }
        else
        {
            ChangeBGM("Sounds/smb_flagpole", false);

            hasReachedGoal = true;
            anim.SetBool("hug", true);
            anim.SetFloat("speed", 0);
            transform.position = Vector3.right * (goal.transform.position.x + 0.2f) + Vector3.up * transform.position.y;

            flagBottomPos = Vector3.right * (goal.transform.position.x + 0.2f) + Vector3.up * 3;

            body.velocity = Vector3.zero;
            body.isKinematic = true;

            goal.GetComponentInParent<Animator>().SetTrigger("down");

            Invoke("BeginGotoCastle", 1.55f);
        }
    }

    void StageClear()
    {
        gameObject.SetActive(false);
    }

    void TryPickupItem(GameObject itemObject)
    {
        ItemMarker itemMarker = itemObject.GetComponent<ItemMarker>();

        if(itemMarker)
        {
            PickupItem(itemMarker.itemType);
            Destroy(itemObject);
        }
    }

    void PickupItem(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Mushroom_Red:
                BeginChangeState(MARIO_BIG);
                break;
            case ItemType.Flower:
                if(state == MARIO_SMALL) BeginChangeState(MARIO_BIG);
                else BeginChangeState(MARIO_FIRE);
                break;
            default:
                break;
        }
    }
}
