using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : MonoBehaviour
{
    [Range(0,20)]
    public float movePower = 10;

    [Range(0, 1000)]
    public float jumpPower = 1000;

    private bool isBreaking;
    private bool isOnGround;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //Debug.Log("左右方向轴：" + h);
        //Debug.Log("上下方向轴：" + v);

        Vector3 force = Vector3.right * h;

        if(h > 0)
        {
            transform.rotation = Quaternion.identity;
        }
        else if(h < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        GetComponent<Rigidbody2D>().AddForce(force * movePower);
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

        if(Input.GetKeyDown(KeyCode.K))
        {
            if(isOnGround)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpPower, ForceMode2D.Impulse);
                GetComponent<Rigidbody2D>().gravityScale = 5;
                isOnGround = false;
            }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);

        if(collision.contacts[0].normal.y > 0)
        {
            Debug.Log("踩到地面");
            GetComponent<Rigidbody2D>().gravityScale = 1;
            isOnGround = true;
        }
    }
}
