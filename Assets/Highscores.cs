using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highscores : MonoBehaviour
{
    const string privateCode = "o_zg_vcwNUCVy7KrTZK_eAJQ0CsgutiEiZbn4HzfJejA";
    const string publicCode = "5f7a19bceb371809c465166a";
    const string webURL = "http://dreamlo.com/lb/";

    string usernameTyped;
    int gameScore;

    HighScore[] highscoreList;
    public Leaderboard lb;


    public Highscores singleton;

    public void AddNewHighScore(string username, int score)
    {
        StartCoroutine(UploadNewScore(username, score));
    }
    public void DownloadTheScores()
    {
        StartCoroutine(DownLoadScores());
    }

    public HighScore[] GetMeTheList()
    {
        return highscoreList;
    }

    IEnumerator UploadNewScore(string username, int score)
    {
        WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score);
        yield return www;

        if(string.IsNullOrEmpty(www.error))
        {
            Debug.Log("Added Success");
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    IEnumerator DownLoadScores()
    {
        WWW www = new WWW(webURL + publicCode + "/pipe");
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            ForMatHighScores(www.text);
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    private void ForMatHighScores(string TextStream)
    {
        string[] entries = TextStream.Split(new char[] { '\n'}, System.StringSplitOptions.RemoveEmptyEntries);
        highscoreList = new HighScore[entries.Length];
        for (int i = 0; i < entries.Length; i++)
        {
            string[] entryInfo = entries[i].Split(new char[] { '|'} );
            string username = entryInfo[0];
            int score = int.Parse( entryInfo[1]);

            highscoreList[i] = new HighScore(username, score);
        }

        lb.Updatelist(highscoreList);
    }

    public struct HighScore
    {
        public string username;
        public int score;

        public HighScore(string _username, int _score)
        {
            username = _username;
            score = _score;
        }
    };

    public void prepHighScoreName(string uname)
    {
        usernameTyped = uname;
    }
    public void prepHighScoreScore(int score)
    {
        gameScore = score;
    }

    public void PushScore()
    {
        Debug.Log(usernameTyped + "||" + gameScore);
            AddNewHighScore(usernameTyped, gameScore);
    }
}
