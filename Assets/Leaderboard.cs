using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    public Highscores hs;
    public Text[] hsText;
    Highscores.HighScore[] hsList;

    private void Start()
    {
        gameObject.SetActive(false);
    }


    // Update is called once per frame
    void OnEnable()
    {
        for (int i = 0; i < hsText.Length; i++)
        {
            hsText[i].text = i + 1 + " - GettingScores";
        }

        StartCoroutine(RefreshHS());
    }

    public void OnScoresDownloaded(Highscores.HighScore[] h)
    {
        for (int i = 0; i < hsText.Length; i++)
        {
            hsText[i].text = i + 1 + ". ";
            if (h.Length > i)
            {
                hsText[i].text += h[i].username + " : " + h[i].score;
            }
        }
    }

    public void Updatelist(Highscores.HighScore[] ls)
    {
        hsList = ls;
        OnScoresDownloaded(hsList);
    }

    IEnumerator RefreshHS()
    {
        while(true)
        {
            hs.DownloadTheScores();
            yield return new WaitForSeconds(30f);
        }
    }
}
