using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cleanup : MonoBehaviour
{
    public bool isShield = false;
    public bool isBullet = false;
    PlayerCanvas playercan;
    Rigidbody rb;

    private void Start()
    {
        playercan = PlayerCanvas.singleton;
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Danger>())
        {
            other.transform.localScale = Vector3.zero;
            other.GetComponent<Danger>().time = 0f;
            other.GetComponent<Danger>().rb.velocity = Vector3.zero;
            other.gameObject.SetActive(false);

            if(isShield && playercan)
            {
                playercan.score += 100;
                gameObject.transform.localScale = Vector3.zero;
                GameObject.FindGameObjectWithTag("Player").GetComponent<Playsounds>().PlaySound(1);
            }
            else if(isShield && !playercan)
            {
                playercan = PlayerCanvas.singleton;
                playercan.score += 100;
                GameObject.FindGameObjectWithTag("Player").GetComponent<Playsounds>().PlaySound(1);
                gameObject.transform.localScale = Vector3.zero;
            }

            if (isBullet && playercan)
            {
                playercan.score += 100;
                Destroy(this.gameObject);
            }
            else if (isShield && !playercan)
            {
                playercan = PlayerCanvas.singleton;
                playercan.score += 100;
                Destroy(this.gameObject);
            }
        }
        if (other.GetComponent<BasicPickup>())
        {
            other.transform.localScale = Vector3.zero;
            other.GetComponent<BasicPickup>().time = 0f;
            other.GetComponent<BasicPickup>().rb.velocity = Vector3.zero;
            other.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if(!isBullet)
        {
            return;
        }

        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.forward * 25 ,ForceMode.VelocityChange);
    }
}
