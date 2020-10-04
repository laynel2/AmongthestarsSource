using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Vector2 inputs;
    public float speed;
    float speedreference;
    RaycastHit hit;
    Vector3 offet;
    public int decimalRounding = 2;
    public float distanceCorrectionThreshold = 0.1f;
    public float inputSwap = -2f;
    float verticaldot;
    bool linejump;
    public LayerMask loopcheck;

    Vector3 goalposition;
    Quaternion goalrot;

    public GameObject thrusters;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        UpdateControls(PlayerPrefs.GetInt("InvertCheck"));
        speedreference = speed;
    }

    private void Update()
    {
        GetInputs();

        verticaldot = Vector3.Dot(transform.up, Vector3.up);

        if (verticaldot <= inputSwap)
        {
            speed = -speedreference;
        }
        else if (verticaldot > inputSwap)
        {
            speed = speedreference;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!linejump)
        {
            rb.velocity = (transform.right * inputs.x * Time.deltaTime * speed);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }


        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            transform.rotation = Quaternion.LookRotation(transform.forward, hit.normal);
            //Make this smoother please
            offet = Round(Vector3.Normalize(transform.position - hit.point), decimalRounding) * 0.5f;

            if (Vector3.Distance(transform.position, hit.point + offet) > distanceCorrectionThreshold)
            {
                if(!linejump)
                transform.position = hit.point + offet;
            }
        }

        if (linejump)
        {
            float t = Time.deltaTime * 5;
            transform.position = Vector3.Lerp(transform.position, goalposition, t);
            transform.rotation = Quaternion.Lerp(transform.rotation, goalrot, t);

            if(Vector3.Distance(transform.position, goalposition) < 0.3f)
            {
                linejump = false;
                transform.position = goalposition;
                transform.rotation = goalrot;
            }
        }

        thrusters.gameObject.SetActive(linejump);
    }

    void GetInputs()
    {
        inputs.x = Input.GetAxisRaw("Horizontal");
        inputs.y = Input.GetAxisRaw("Vertical");

        if(Input.GetButtonDown("Fire1") && !linejump)
        {
            linejump = true;
            if (Physics.Raycast(transform.position, transform.up, out RaycastHit opposite, 8f, loopcheck))
            {
                if(Vector3.Dot(opposite.normal, transform.forward) > 0.1f)
                {
                    linejump = false;
                    return;
                }

                goalposition = opposite.point - offet;
                goalrot = Quaternion.LookRotation(transform.forward, opposite.normal);
            }
            else
            {
                linejump = false;
            }
        }        
    }

    Vector3 Round(Vector3 v3, int decimalRounding)
    {
        float multiplier = 1;
        for (int i = 0; i < decimalRounding; i++)
        {
            multiplier *= 10f;
        }
        return new Vector3(
            Mathf.Round(v3.x * multiplier) / multiplier,
            Mathf.Round(v3.y * multiplier) / multiplier,
            Mathf.Round(v3.z * multiplier) / multiplier);
    }
    public void UpdateControls(int i)
    {
        inputSwap = i * -2;
    }

}
