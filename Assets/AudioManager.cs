using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager singleton;
    int musicplayers = 3;
    private AudioSource[] audioSources = new AudioSource[3];
    private double nextEventTime;
    private int flip = 0;
    bool running;
    public AudioClip intro;

    public AudioClip[] clips = new AudioClip[2];
    public float bpm = 124;
    public int numBeatsPerSegment = 260;

    public bool ambience;
    public AudioClip ambientTrack;
    AudioSource am;

    int repeatcounts;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < musicplayers; i++)
        {
            GameObject child = new GameObject("Player");
            child.transform.parent = gameObject.transform;
            audioSources[i] = child.AddComponent<AudioSource>();
        }

        audioSources[1].PlayOneShot(intro);
        nextEventTime = AudioSettings.dspTime + 60.0f / bpm * 36;
        running = true;
        am = audioSources[2];
        am.loop = true;
        am.clip = ambientTrack;
        am.volume = 0;
        am.Play();
    }

    void Awake()
    {
        if(singleton!= null)
        {
            DestroyImmediate(this);
        }
        else
        {
            singleton = this;
        }
    }

    void Update()
    {
        if (!running)
        {
            return;
        }

        double time = AudioSettings.dspTime;

        if (time + 1.0f > nextEventTime)
        {
            audioSources[flip].clip = clips[flip];
            audioSources[flip].PlayScheduled(nextEventTime);
            ambience = false;

            if(repeatcounts > Random.Range(1,3))
            {
                nextEventTime += 60.0f / bpm * numBeatsPerSegment * Random.Range(2,4);
                ambience = true ;
                repeatcounts = 0;
            }
            nextEventTime += 60.0f / bpm * numBeatsPerSegment;
            flip = 1 - flip;
            repeatcounts++;
        }

        if(ambience)
        {
            am.volume = Mathf.Lerp(am.volume,1f,Time.deltaTime * 0.5f);
        }
        else
        {
            am.volume = Mathf.Lerp(am.volume, 0f, Time.deltaTime * 0.5f);
        }
    }

    public void Mute(int v)
    {
        for (int i = 0; i < 2; i++)
        {
            audioSources[i].volume = v;
        }
    }
}
