using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : Block
{
    protected GameObject debrisEffect;

    // Start is called before the first frame update
    void Start()
    {
        OnStart();
    }

    protected override void OnStart()
    {
        debrisEffect = Resources.Load<GameObject>("Prefabs/Effects/Debris_0");
        base.OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnHit(GameObject hit)
    {
        Mario mario = hit.GetComponent<Mario>();

        if(mario)
        {
            if (mario.state == 0)
            {
                anim.SetTrigger("hit");
            }
            else
            {
                BreakBlock();
            }
        }
    }

    protected override void BreakBlock()
    {
        base.BreakBlock();
        Debug.Log("碎");
        Instantiate(debrisEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }
}
