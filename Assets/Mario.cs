using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isOnGround = CheckGround();

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

    bool CheckGround()
    {
        bool result = false;
        body.gravityScale = 5;

        Physics2D.queriesStartInColliders = false;

        // 在前方有没有地面
        bool frontHasGround = (Physics2D.Raycast(transform.position + Vector3.right * 0.53f, Vector2.down, 0.55f).transform != null);

        Debug.DrawLine(transform.position + Vector3.right * 0.53f, transform.position + Vector3.right * 0.53f + Vector3.down * 0.55f, Color.yellow);

        // 在后方有没有地面
        bool backHasGround = (Physics2D.Raycast(transform.position - Vector3.right * 0.53f, Vector2.down, 0.55f).transform != null);

        Debug.DrawLine(transform.position - Vector3.right * 0.53f, transform.position - Vector3.right * 0.53f + Vector3.down * 0.55f, Color.yellow);

        // 在正下方有没有地面
        bool midHasGround = (Physics2D.Raycast(transform.position, Vector2.down, 0.55f).transform != null);

        Debug.DrawLine(transform.position , transform.position + Vector3.down * 0.55f, Color.yellow);

        if (frontHasGround || backHasGround || midHasGround)
        {
            result = true;
            body.gravityScale = 1;
        }

        Physics2D.queriesStartInColliders = true;

        return result;
    }
}
