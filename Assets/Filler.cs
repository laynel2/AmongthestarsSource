using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Filler : MonoBehaviour
{
    public Image fillImage;
    public bool filling;
    public float fillTime = 2.0f;

    public bool invert;
    bool done;
    public bool muter;
    int mutenum = 0;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            filling = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            filling = false;
        }
    }

    private void Update()
    {
        if(filling)
        {
            fillImage.fillAmount += 1.0f / fillTime * Time.deltaTime;
        }
        else
        {
            done = false;
            fillImage.color = Color.white;
            fillImage.fillAmount -= 1.0f / fillTime * Time.deltaTime * 3;
        }

        if(fillImage.fillAmount >= 1f && !done)
        {
            done = true;
            DoEvent(invert);
        }
    }

    void DoEvent(bool i)
    {
        if (!i)
        {
            SceneManager.LoadScene(1);
        }
        else if(muter)
        {
            fillImage.color = Color.green;
            AudioManager.singleton.Mute(mutenum);
            mutenum = 1 - mutenum;
            return;
        }

        else if(invert)
        {
            fillImage.color = Color.green;
            if (PlayerPrefs.GetInt("InvertCheck") > 0)
            {
                PlayerPrefs.SetInt("InvertCheck", -1);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().UpdateControls(-1);
                return;
            }

            if(PlayerPrefs.GetInt("InvertCheck") < 0)
            {
                PlayerPrefs.SetInt("InvertCheck", 1);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().UpdateControls(1);
                return;
            }
            else
            {
                PlayerPrefs.SetInt("InvertCheck", -1);
                Debug.Log("error");
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().UpdateControls(1);
                return;
            }
        }


    }

}
