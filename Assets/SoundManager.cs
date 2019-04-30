using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioClip[] audioClips;

    // Start is called before the first frame update
    void Start()
    {
        audioClips = Resources.LoadAll<AudioClip>("Sounds/");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
