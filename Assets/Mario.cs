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

    private Stopwatch jumpTimer = new Stopwatch(0.2f);

    // Start is called before the first frame update
    void Start()
    {
        
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

        GetComponent<Rigidbody2D>().AddForce(force * movePower);


        if (isOnGround)
        {
            float speed = GetComponent<Rigidbody2D>().velocity.magnitude;

            GetComponent<Animator>().SetFloat("speed", speed);

            if (ShouldBreak(GetComponent<Rigidbody2D>().velocity.x, h))
            {
                if (!isBreaking)
                {
                    Debug.Log("Break");
                    isBreaking = true;
                    GetComponent<Animator>().ResetTrigger("break");
                    GetComponent<Animator>().SetTrigger("break");
                }
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                jumpTimer.Start();
            }

            if(Input.GetKeyUp(KeyCode.K))
            {
                if(jumpTimer.isCounting)
                {
                    jumpTimer.Stop();
                    float jumpPower = Mathf.Lerp(jumpMinPower, jumpMaxPower, jumpTimer.time);
                    GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpPower, ForceMode2D.Impulse);
                }
            }

            if (jumpTimer.TimeSinceStart() > 0.17f)
            {
                if(jumpTimer.isCounting)
                {
                    jumpTimer.Stop();
                    GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpMaxPower, ForceMode2D.Impulse);
                }
            }
        }

        GetComponent<Animator>().SetBool("grounded", isOnGround);
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
        Physics2D.queriesStartInColliders = false;
        GetComponent<Rigidbody2D>().gravityScale = 5;

        bool frontHasGround = (Physics2D.Raycast(transform.position + Vector3.right * 0.5f, Vector2.down, 0.55f).transform != null);
        bool backHasGround = (Physics2D.Raycast(transform.position - Vector3.right * 0.5f, Vector2.down, 0.55f).transform != null);
        bool midHasGround = (Physics2D.Raycast(transform.position, Vector2.down, 0.55f).transform != null);

        if (frontHasGround || backHasGround || midHasGround)
        {
            result = true;
            GetComponent<Rigidbody2D>().gravityScale = 1;
        }

        Physics2D.queriesStartInColliders = true;

        return result;
    }
}
