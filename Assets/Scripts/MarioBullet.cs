using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioBullet : MonoBehaviour
{
    [Range(0,15)]
    public float speed = 1;

    [Range(0, 10)]
    public float bounceGravityScale = 5;

    [Range(0, 15)]
    public float bounceUpSpeed = 9;

    private Rigidbody2D body;

    private List<Vector3> trackList = new List<Vector3>();

    private AudioClip explodeClip;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0;
        body.velocity = (transform.right - transform.up).normalized * speed;// (1,0,0) - (0,1,0) = (1,-1,0)

        explodeClip = Resources.Load<AudioClip>("Sounds/smb_bump");
    }

    // Update is called once per frame
    void Update()
    {
        trackList.Add(transform.position);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] contacts = collision.contacts;

        Enemy enemy = collision.transform.GetComponent<Enemy>();

        if(enemy)
        {
            enemy.OnHit(contacts[0].normal);

            Explode();
            return;
        }
        else
        {
            for (int i = 0; i < contacts.Length; i++)
            {
                Debug.Log("i:" + contacts[i].normal);

                if (Mathf.Abs(contacts[i].normal.y) < 0.5f)
                {
                    Explode();
                    return;
                }
            }

            body.gravityScale = bounceGravityScale;
            body.velocity = transform.right * Mathf.Abs(body.velocity.x) + transform.up * bounceUpSpeed;
        }
    }

    void Explode()
    {
        body.isKinematic = true;
        body.velocity = Vector3.zero;
        GetComponent<Collider2D>().enabled = false;
        AudioSource.PlayClipAtPoint(explodeClip, Camera.main.transform.position);
        GetComponent<Animator>().SetTrigger("explode");
        Destroy(gameObject,2f);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for(int i = 0; i < trackList.Count; i++)
        {
            Gizmos.DrawSphere(trackList[i], 0.05f);
        }
    }
}
