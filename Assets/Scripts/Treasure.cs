using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public AudioClip hitSound;
    public AudioClip emptySound;
    private int hitCount = 3;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnHit()
    {
        if (hitCount <= 0)  // 金币已经顶空的时候，不播动画，只播顶空声音
        {
            AudioSource.PlayClipAtPoint(emptySound, Camera.main.transform.position);
        }
        else // 还有金币的时候
        {
            // 扣掉金币计数
            hitCount--;

            // 扣掉以后的剩余金币为0的时候，将动画状态机的empty变量设为true，播放顶空动画，否则播放普通顶中动画
            if(hitCount <= 0)
            {
                anim.SetBool("empty", true);
            }

            AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position);
            anim.SetTrigger("hit");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnHit();

        if (collision.contacts[0].normal.y > 0)
        {
            
        }
    }
}
