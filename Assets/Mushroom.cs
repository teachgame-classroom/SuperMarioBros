using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    private Vector3 originPos;
    private Vector3 targetPos;
    private float t;

    private Vector3 velocityX;
    private Vector3 velocity;
    Rigidbody2D body;

    // Update is called once per frame
    void Start()
    {
        originPos = transform.position;
        targetPos = originPos + Vector3.up;
        body = GetComponent<Rigidbody2D>();
        velocityX = Vector3.right * 4;
        velocity = velocityX + Vector3.up * body.velocity.y;
    }

    void Update()
    {
        if(t < 1)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(originPos, targetPos, t);
        }
        else
        {
            body.isKinematic = false;
            body.gravityScale = 2;
            velocity = velocityX + Vector3.up * body.velocity.y;
            body.velocity = velocity;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] contacts = collision.contacts;

        bool changeDirection = false;

        for(int i = 0; i < contacts.Length; i++)
        {
            changeDirection = (velocity.x > 0 && contacts[i].normal.x < 0) || (velocity.x < 0 && contacts[i].normal.x > 0);

            if (changeDirection)
            {
                velocityX.Scale(new Vector3(-1, 1, 1));
                return;
            }
        }
    }
}
