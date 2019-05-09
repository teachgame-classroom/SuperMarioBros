using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotate : MonoBehaviour
{
    public float popupSpeed = 10;

    private Vector3 originPos;

    // Start is called before the first frame update
    void Awake()
    {
        originPos = transform.position;
        Pop();
    }

    void OnEnable()
    {
        Pop();
    }

    private void Pop()
    {
        transform.position = originPos;
        GetComponent<Rigidbody2D>().velocity = Vector3.up * popupSpeed;
        DeactiveDelay(0.55f);
    }

    void DeactiveDelay(float delay)
    {
        Invoke("DoDeactive", delay);
    }

    void DoDeactive()
    {
        gameObject.SetActive(false);
    }
}
