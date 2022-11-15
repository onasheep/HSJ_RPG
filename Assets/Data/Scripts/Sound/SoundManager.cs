using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip BGM;
    public Player myPlayer;
    public AudioClip GameOver;
    AudioSource myAudio;

    // Update is called once per frame
    void Awake()
    {
        myAudio = this.GetComponent<AudioSource>();
        myAudio.volume = 0.2f;
    }

    public void OnPlay(AudioClip myClip)
    {
        myAudio.clip = myClip;
        myAudio.Play();
    }
}
