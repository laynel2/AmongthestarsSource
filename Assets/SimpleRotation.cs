using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    public bool randomize = false;
    public int min1 = -250, min2 = -125;
    public float x, y, z = 0f;
    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);

        if (randomize)
        {
            x += Random.Range(Random.Range(min1, min2), Random.Range(-min2, -min1));
            y += Random.Range(Random.Range(min1, min2), Random.Range(-min2, -min1));
            z += Random.Range(Random.Range(min1, min2), Random.Range(-min2, -min1));
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(x * Time.deltaTime, y * Time.deltaTime, z * Time.deltaTime);
    }
}
