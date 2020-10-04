using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardReveal : MonoBehaviour
{

    public GameObject leaderboard;

    private void Awake()
    {
        leaderboard.SetActive(false);
    }

    private void Start()
    {
        leaderboard.SetActive(false);
    }

    // Update is called once per frame
    public void ShowLeaderboard()
    {
        leaderboard.SetActive(true);
    }

    public void HideLeaderboard()
    {
        leaderboard.SetActive(false);
    }
}
