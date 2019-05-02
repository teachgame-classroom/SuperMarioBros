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

    private bool isBreaking;
    private bool isOnGround;
    private bool isJumpingUp;

    private Rigidbody2D body;
    private Animator anim;

    private AudioClip dieClip;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dieClip = Resources.Load<AudioClip>("Sounds/smb_mariodie");
    }

    // Update is called once per frame
    void Update()
    {
        if(state != MARIO_DIE)
        {
            bool isOnGround = CheckGroundAndEnemy();
            bool isHurt = CheckHurt();

            if(isHurt)
            {
                state = MARIO_DIE;
                body.velocity = Vector3.zero;
                body.isKinematic = true;
                anim.SetBool("dead", true);
                Camera.main.GetComponent<AudioSource>().clip = dieClip;
                Camera.main.GetComponent<AudioSource>().loop = false;
                Camera.main.GetComponent<AudioSource>().Play();
                Invoke("DieFall", 0.5f);
                //AudioSource.PlayClipAtPoint(dieClip, Camera.main.transform.position);
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

            body.AddForce(force * movePower);

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
                if(body.velocity.y < 0)
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
        }
        else
        {
        }
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
        bool result = ThreeLineCast(transform.position, Vector3.right * 0.4f, Vector2.down, 0.55f, out hitTrans);

        if(result)
        {
            for(int i = 0; i < hitTrans.Length; i++)
            {
                if(hitTrans[i])
                {
                    Enemy enemy = hitTrans[i].GetComponent<Enemy>();

                    if (enemy)
                    {
                        enemy.OnHit();
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
        bool rightHurt = CheckHurt(Vector3.up * 0.55f, Vector3.right, 0.4f);
        bool leftHurt = CheckHurt(Vector3.up * 0.55f, Vector3.left, 0.4f);
        bool topHurt = CheckHurt(Vector3.right * 0.4f, Vector3.up, 0.55f);

        return topHurt;

        //return rightHurt || leftHurt || topHurt;
    }

    bool CheckHurt(Vector3 offset, Vector3 direction, float distance)
    {
        Transform[] hitTrans;
        bool hurtResult = false;

        bool lineCastResult = ThreeLineCast(transform.position, offset, direction, distance, out hitTrans);

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
}
