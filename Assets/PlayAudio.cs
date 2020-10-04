using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public AudioClip clip;
    AudioSource AS;

    private void Start()
    {
        AS = GetComponent<AudioSource>();
    }

    public void Sound()
    {
        if (clip)
            AS.PlayOneShot(clip);
        else
            AS.Play();
    }
}
