using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip BGM;
    public Player myPlayer;
    public AudioClip GameOver;

    // Update is called once per frame
    void Awake()
    {
        this.GetComponent<AudioSource>().volume = 0.2f;
    }
}
