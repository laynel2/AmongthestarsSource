using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public bool isActive;
    public float fireRate = 0.125f;
    public GameObject bullet;
    float time;
    float ellaspedtime;
    public float length = 3f;
    public static Gun singleton;
    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
    }

    private void Awake()
    {
        if (singleton != null)
        {
            Destroy(this);
        }
        else
        {
            singleton = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            time += Time.deltaTime;
            ellaspedtime+= Time.deltaTime;

            if (time>= fireRate)
            {
                time = 0f;
                GameObject bull = Instantiate(bullet,transform.position,Quaternion.identity);
                Destroy(bull, 3f);
                //bull.Fire();
            }

            if(ellaspedtime > length)
            {
                ellaspedtime = 0f;
                isActive = false;
            }
        }
    }
}
