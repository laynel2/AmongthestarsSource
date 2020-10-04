using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPickup : MonoBehaviour
{
    PlayerCanvas playercan;
    public float time = 0;
    Vector3 goalScale;
    public Rigidbody rb;
    public float speed = 500f;
    GameObject player;
    public float distanceThreshold = 1.5f;
    bool magnet;
    public bool powerUp;
    public GameObject shield;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        playercan = PlayerCanvas.singleton;
        if(powerUp)
        shield = player.GetComponentInChildren<cleanup>().gameObject;
    }

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        goalScale = new Vector3(0.5f, 0.5f, 0.5f);
        transform.localScale = Vector3.zero;
        time = 0f;
    }
    void PickUp()
    {
        if (powerUp)
            shield = player.GetComponentInChildren<cleanup>().gameObject;

        if (shield.transform.localScale.magnitude > 0)
        {
            Gun.singleton.isActive = true;
            player.GetComponent<Playsounds>().PlaySound(4);
        }
        else
        {
            player.GetComponent<Playsounds>().PlaySound(4);
            shield.gameObject.transform.localScale = new Vector3 (1.25f,1.25f,1.25f);
        }
        gameObject.SetActive(false);
    }
    void PickUp(int i)
    {
        if(playercan == null)
        {
            playercan = PlayerCanvas.singleton;
        }

        playercan.score += i;
        player.GetComponent<Playsounds>().PlaySound(0);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (!powerUp)
            {
                PickUp(200);
            }
            else
            {
                PickUp();
            }
        }
    }

    private void FixedUpdate()
    {
        if(player == null)
        {
            player = GameObject.FindWithTag("Player");
            return;

        }

        magnet = Vector3.Distance(player.transform.position, transform.position) < distanceThreshold;

        if (time < 1f)
        {
            time += Time.deltaTime * 0.5f;
        }
        if (time > 0.2f && !magnet)
        {
            rb.velocity = Vector3.forward * Time.deltaTime * -speed;
        }

        transform.localScale = Vector3.Lerp(transform.localScale, goalScale, time);

        if(magnet)
        {
            rb.velocity = Vector3.zero ;
            Vector3 goalPos = player.transform.position;
            transform.position = Vector3.Lerp(transform.position, goalPos, Time.deltaTime * 4);
        }
    }
}
