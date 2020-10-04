using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danger : MonoBehaviour
{
    public float time = 0;
    Vector3 goalScale;
    public Rigidbody rb;
    public float speed = 350f;
    SpawnManager parent;

    private void FixedUpdate()
    {
        if (time < 1f)
        {
            time += Time.deltaTime * 0.5f;
        }
        if(time > 0.2f)
        {
            rb.velocity = Vector3.forward * Time.deltaTime * -speed;
        }

        transform.localScale = Vector3.Lerp(transform.localScale, goalScale, time);

        //rb.velocity = Vector3.forward * Time.deltaTime * -speed;
    }

    void OnEnable()
    {
        goalScale = new Vector3 (0.5f, 0.5f, 0.5f);
        transform.localScale = Vector3.zero;
        time = 0f;

        CheckSpeed();

        if(rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && other.GetComponent<PlayerLives>())
        {
            other.GetComponent<PlayerLives>().LoseLife();
            //PlayFx Here instead
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        parent = GetComponentInParent<SpawnManager>();
    }

    void CheckSpeed()
    {
        if(parent != null)
        speed = parent.baseSpeed;
    }
}
