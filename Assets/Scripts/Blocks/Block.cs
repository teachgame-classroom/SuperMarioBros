using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    protected Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        OnStart();
    }

    protected virtual void OnStart()
    {
        anim = GetComponent<Animator>();
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
            }
        }
    }
}
