using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IScore, IHealth
{
    private Animator anim;
    private Collider2D col;
    private Rigidbody2D body;
    private AudioClip stompClip;
    private AudioClip kickClip;


    public int score { get { return 100; } }

    public int maxHp { get { return _maxHp; } set { _maxHp = value; } }
    public int _maxHp = 1;

    public int currentHp { get { return _currentHp; } }
    private int _currentHp = 1;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        body = GetComponent<Rigidbody2D>();
        stompClip = Resources.Load<AudioClip>("Sounds/smb_stomp");
        kickClip = Resources.Load<AudioClip>("Sounds/smb_kick");

        _currentHp = _maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStomp()
    {
        anim.SetTrigger("die");
        col.enabled = false;
        body.isKinematic = true;

        AudioSource.PlayClipAtPoint(stompClip, Camera.main.transform.position);

        // 延时1秒后销毁自己
        Destroy(gameObject, 1f);
    }

    public void OnHit(Vector3 hitNormal)
    {
        anim.enabled = false;
        col.enabled = false;
        transform.position += Vector3.up;
        transform.localScale = new Vector3(1, -1, 1);
        body.gravityScale = 8;
        body.constraints = RigidbodyConstraints2D.None;

        Vector3 fallVelocity = Vector3.right * 3 + Vector3.up * 15;

        if(hitNormal.x > 0)
        {
            fallVelocity = Vector3.right * -3 + Vector3.up * 15;
        }

        body.velocity = fallVelocity;

        AudioSource.PlayClipAtPoint(kickClip, Camera.main.transform.position);

        // 延时1秒后销毁自己
        Destroy(gameObject, 3f);
    }

    public void ChangeHp(int amount)
    {
        _currentHp = Mathf.Clamp(amount, 0, _maxHp);

        if (_currentHp == 0)
        {
            OnStomp();
        }
    }

    public void Hit(int amount)
    {
        _currentHp = Mathf.Clamp(_currentHp - amount, 0, _maxHp);

        if (_currentHp == 0)
        {
            OnStomp();
        }
    }

    public void Heal(int amount)
    {
        _currentHp = Mathf.Clamp(_currentHp + amount, 0, _maxHp);
    }

    public void Die()
    {
        Debug.Log("Enemy Die");
        if (_currentHp > 0)
        {
            _currentHp = 0;
            GameController.instance.AddScore(this);
            OnStomp();
        }
    }
}
