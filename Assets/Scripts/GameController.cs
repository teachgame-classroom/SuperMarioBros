using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    private int totalScore;

    public float timer = 5f;
    public bool TimesUp { get { return timer <= 0; } }

    private Mario mario;

    void Awake()
    {
        instance = this;
        mario = GameObject.Find("Mario").GetComponent<Mario>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (!TimesUp)
        //{
        //    timer -= Time.deltaTime;
        //}
        //else
        //{
        //    Debug.Log("Times Up");

        //    mario.Die();
        //}

        if (Input.GetKeyDown(KeyCode.T))
        {
            GameObject[] gos = FindObjectsOfType<GameObject>();

            for(int i = 0; i < gos.Length; i++)
            {
                IHealth health = gos[i].GetComponent<IHealth>();
                if (health != null)
                {
                    health.Die();
                }
            }
        }
    }

    public void AddScore(IScore s)
    {
        totalScore += s.score;
        Debug.Log("Total Score:" + totalScore);
    }
}
