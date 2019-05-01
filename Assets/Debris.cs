using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    public AudioClip soundClip;
    private GameObject[] debris = new GameObject[2];
    private float y_minSpeed = 10;
    private float y_maxSpeed = 15;
    private float x_speed = 2;

    // Start is called before the first frame update
    void Start()
    {
        debris[0] = Resources.Load<GameObject>("Prefabs/Effects/Debris_1");
        debris[1] = Resources.Load<GameObject>("Prefabs/Effects/Debris_2");

        SpawnDebris();

        AudioSource.PlayClipAtPoint(soundClip, Camera.main.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnDebris()
    {
        GameObject debris1 = Instantiate(debris[0], transform.position, Quaternion.identity);
        debris1.GetComponent<Rigidbody2D>().velocity = new Vector2(x_speed, y_minSpeed);

        GameObject debris2 = Instantiate(debris[0], transform.position, Quaternion.identity);
        debris2.GetComponent<Rigidbody2D>().velocity = new Vector2(x_speed, y_maxSpeed);
        
        GameObject debris3 = Instantiate(debris[0], transform.position, Quaternion.identity);
        debris3.GetComponent<Rigidbody2D>().velocity = new Vector2(-x_speed, y_minSpeed);

        GameObject debris4 = Instantiate(debris[0], transform.position, Quaternion.identity);
        debris4.GetComponent<Rigidbody2D>().velocity = new Vector2(-x_speed, y_maxSpeed);
    }
}
