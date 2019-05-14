using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Actor, IHealth
{
    protected Animator anim;

    public int maxHp { get { return maxHitCount; } set { maxHitCount = value; } }
    public int maxHitCount = 1;
    
    public int currentHp { get { return currentHitCount; } }
    protected int currentHitCount;

    // Start is called before the first frame update
    void Start()
    {
        OnStart();
    }

    protected virtual void OnStart()
    {
        currentHitCount = maxHitCount;
        anim = GetComponent<Animator>();
        //BreakBlock();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void OnHit(GameObject hit)
    {

    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit");

        for(int i = 0; i < collision.contacts.Length; i++)
        {
            Color lineColor = Color.red;

            if(collision.contacts[i].normal.y > 0)
            {
                lineColor = Color.green;
            }

            Debug.DrawLine(collision.contacts[i].point, collision.contacts[i].point + collision.contacts[i].normal, lineColor, 3f);

            if (collision.contacts[0].normal.y > 0)
            {
                OnHit(collision.gameObject);

                // 有多个碰撞接触点时，只调用一次命中处理，防止顶一次触发多次顶取
                return;
            }
        }
    }

    protected virtual void BreakBlock()
    {

    }

    public void ChangeHp(int amount)
    {
        throw new System.NotImplementedException();
    }

    public void Hit(int amount)
    {
        throw new System.NotImplementedException();
    }

    public void Heal(int amount)
    {
        throw new System.NotImplementedException();
    }

    public void Die()
    {
        BreakBlock();
    }
}
