using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : Actor
{
    private GameObject fireworksPrefab;
    private AudioClip fireworksClip;

    private Vector3 fireworksRectOffset = new Vector3(-3, 5, 0);
    private Vector3 fireworksRectOrigin;

    private float fireworksRectWidth = 11;
    private float fireworksRectHeight = 5;

    public int maxCount = 3;
    private int counter;

    // Start is called before the first frame update
    void Start()
    {
        fireworksPrefab = Resources.Load<GameObject>("Prefabs/Effects/Fireworks");

        fireworksClip = Resources.Load<AudioClip>("Sounds/smb_fireworks");

        fireworksRectOrigin = transform.position + fireworksRectOffset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            GetComponent<Animator>().SetTrigger("flag");
            InvokeRepeating("PlayFireworks", 4f, 1f);
        }
    }

    void PlayFireworks()
    {
        counter++;

        Vector3 fireworksPos = Vector3.right * Random.Range(fireworksRectOrigin.x, fireworksRectOrigin.x + fireworksRectWidth)
                                + Vector3.up * Random.Range(fireworksRectOrigin.y, fireworksRectOrigin.y + fireworksRectHeight);

        Instantiate(fireworksPrefab, fireworksPos, Quaternion.identity);

        AudioSource.PlayClipAtPoint(fireworksClip, Camera.main.transform.position);

        if(counter >= maxCount)
        {
            CancelInvoke();
        }
    }
}
