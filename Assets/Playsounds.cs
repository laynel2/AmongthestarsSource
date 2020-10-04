using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playsounds : MonoBehaviour
{
    public AudioClip coin,asteroid,damage,die,powerup;
    public AudioSource asource;
    // Start is called before the first frame update
    void Start()
    {
        asource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void PlaySound(int index)
    {
        if(asource.isPlaying)
        {
            return;
        }

        switch (index)
        {
            case 0:
                asource.PlayOneShot(coin);
                break;

            case 1:
                asource.PlayOneShot(asteroid);
                break;

            case 2:
                asource.PlayOneShot(damage);
                break;

            case 3:
                asource.PlayOneShot(die);
                break;

            case 4:
                asource.PlayOneShot(powerup);
                break;


            default:
        break;
        }
    }
}
