using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTools;

public class Activity_SimpleMove : Activity
{
    protected Vector3 originPos;
    protected Vector3 targetPos;
    protected float t;

    protected Vector3 velocityX;
    protected Vector3 velocity;
    protected Rigidbody2D body;


    public Activity_SimpleMove()
    {
    }

    public override void SetOwner(Actor owner)
    {
        base.SetOwner(owner);
        body = owner.GetComponent<Rigidbody2D>();
        velocityX = Vector3.right * 4;
        velocity = velocityX + Vector3.up * body.velocity.y;
        EventManager.RegisterEvent<Actor, Collision2D>("EnemyCollisionEnter", OnEnemyCollisionEnter2D);
        
    }

    public override void Update()
    {
        body.isKinematic = false;
        body.gravityScale = 2;
        velocity = velocityX + Vector3.up * body.velocity.y;
        body.velocity = velocity;
    }

    void OnEnemyCollisionEnter2D(Actor enemy, Collision2D collision)
    {
        if (enemy != owner) return;

        Debug.Log("I Hit Something:" + collision.gameObject.name);

        ContactPoint2D[] contacts = collision.contacts;

        bool changeDirection = false;

        for (int i = 0; i < contacts.Length; i++)
        {            
            changeDirection = (velocity.x > 0 && contacts[i].normal.x < 0) || (velocity.x < 0 && contacts[i].normal.x > 0);

            Debug.Log("vx:" + velocity.x + "," + "normalx:" + contacts[i].normal.x + "," + changeDirection);

            if (changeDirection)
            {
                velocityX.Scale(new Vector3(-1, 1, 1));
                velocity = velocityX + Vector3.up * body.velocity.y;
                body.velocity = velocity;
                return;
            }
        }
    }

}
