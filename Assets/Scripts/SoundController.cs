using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource src;
    public AudioClip sfx1,sfx2,sfx3;

    public void clearSound(){
        src.clip = sfx1;
        src.Play();
    }

    public void place(){
        src.clip = sfx2;
        src.time = 0.455f;
        src.Play();
    }

    public void rotate(){
        src.clip = sfx3;
        src.Play();
    }

}
