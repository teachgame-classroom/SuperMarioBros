using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : Actor, IHealth
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

    private float speed;

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

    public int maxHp { get { return _maxHp; } set { _maxHp = value; } }
    public int _maxHp;

    public int currentHp { get { return _currentHp; } }
    private int _currentHp;

    //private List<Activity> activities = new List<Activity>();

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

        _currentHp = _maxHp;

        InitContainer();

        activities.Add<Activity_Move>();
        activities.Add<Activity_Direction>();

        //activities.Add(new Activity_Move(this, movePower));
        //activities.Add(new Activity_Direction(this));

    }

    // Update is called once per frame
    void Update()
    {
        float h;
        float v;

        bool jumpButton;
        bool fireButton;

        Update_Input(out h, out v, out jumpButton, out fireButton);

        // 马里奥生存
        if (state != MARIO_DIE)
        {
            // 到达旗帜
            if (hasReachedGoal)
            {
                // 旗帜落下前
                if (!goingToCastle)
                {
                    // 自动滑下旗杆
                    transform.position = Vector3.MoveTowards(transform.position, flagBottomPos, Time.deltaTime * 6);
                }
                else
                {
                    // 自动前进到城堡
                    body.AddForce(Vector3.right * movePower);
                }
            }
            else // 通关前
            {
                // Debug变身
                //if (!isChangingState)
                //{
                //    Update_DebugChangeState();
                //}

                // 处于变身暂停状态
                if (isChangingState)
                {
                    Update_ChangeState();
                }

                // 处于无敌状态
                if (invincible)
                {
                    Update_Invincible();
                }

                isOnGround = CheckGroundAndEnemy();
                Debug.Log("OnGround:" + isOnGround);
                bool isHurt = CheckHurt();

                if (isHurt)
                {
                    Update_Hurt();
                }

                activities.Update();

                //foreach (Activity act in activities)
                //{
                //    act.Update();
                //}

                //Update_Direction(h);
                //Update_Move(h);

                if (isOnGround)
                {
                    Update_OnGround(h, jumpButton);
                }
                else
                {
                    Update_Air();
                }

                if (state == MARIO_FIRE)
                {
                    if (fireButton)
                    {
                        Shoot();
                    }
                }
            }
        }

        Update_Animator();
    }

    private void Update_Air()
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

    private void Update_OnGround(float h, bool jumpButton)
    {
        speed = body.velocity.magnitude;
        isBreaking = ShouldBreak(body.velocity.x, h);

        if (jumpButton)
        {
            body.AddForce(Vector3.up * jumpMaxPower, ForceMode2D.Impulse);
            isJumpingUp = true;
        }
    }

    private void Update_Animator()
    {
        anim.SetFloat("speed", speed);
        anim.SetBool("grounded", isOnGround);

        anim.SetBool("break", isBreaking);
    }

    private void Update_Move(float h)
    {
        Vector3 force = Vector3.right * h;

        if (Time.timeScale > 0)
        {
            body.AddForce(force * movePower);
        }
    }

    private void Update_Direction(float h)
    {
        if (h > 0)
        {
            transform.rotation = Quaternion.identity;
        }
        else if (h < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void Update_Input(out float h, out float v, out bool jumpButton, out bool fireButton)
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        jumpButton = Input.GetKeyDown(KeyCode.K);
        fireButton = Input.GetKeyDown(KeyCode.J);
    }

    private void Update_Hurt()
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

    private void Update_DebugChangeState()
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

    private void Update_Invincible()
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

    void Update_ChangeState()
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

        IScore s = itemObject.GetComponent<IScore>();

        if(s != null)
        {
            GameController.instance.AddScore(s);
        }
        else
        {
            Debug.Log("这个东西不加分");
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

    public void ChangeHp(int amount)
    {
        _currentHp = Mathf.Clamp(amount, 0, _maxHp);

        if(_currentHp == 0)
        {
            Die();
        }
    }

    public void Hit(int amount)
    {
        _currentHp = Mathf.Clamp(_currentHp - amount, 0, _maxHp);

        if (_currentHp == 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        _currentHp = Mathf.Clamp(_currentHp + amount, 0, _maxHp);
    }

    public void Die()
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
}
