using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator anim;
    private Collider2D col;
    private Rigidbody2D body;
    private AudioClip stompClip;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        body = GetComponent<Rigidbody2D>();
        stompClip = Resources.Load<AudioClip>("Sounds/smb_stomp");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHit()
    {
        anim.SetTrigger("die");
        col.enabled = false;
        body.isKinematic = true;

        AudioSource.PlayClipAtPoint(stompClip, Camera.main.transform.position);

        // 延时1秒后销毁自己
        Destroy(gameObject, 1f);
    }
}
