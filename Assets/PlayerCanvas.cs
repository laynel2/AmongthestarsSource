using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    public static PlayerCanvas singleton;
    public GameObject life;
    public GameObject[] lives;
    public int remainingLives;
    public float score;
    public bool gameover;
    // Start is called before the first frame update
    void Awake()
    {
        if (singleton)
        {
            Destroy(this);
        }
        else
        {
            singleton = this;
        }
    }

    private void Start()
    {
        remainingLives = PlayerLives.singleton.lives;
        SpawnLives();
    }

    void SpawnLives()
    {
        if (remainingLives > 0)
        {
            lives = new GameObject[remainingLives];
            for (int i = 0; i < remainingLives; i++)
            {
                GameObject l = Instantiate(life, transform);
                lives[i] = l;
                Vector3 lpos = l.GetComponent<RectTransform>().anchoredPosition;
                lpos.x += 75 * i; ;
                l.GetComponent<RectTransform>().anchoredPosition = lpos;
            }
        }
    }

    public void UpdateLives(int i)
    {
        //test recovering lives please
        if(i > -1)
        lives[i].SetActive(!lives[i].activeInHierarchy);
    }

    private void Update()
    {
        if (!gameover)
        {
            score += Time.deltaTime * 100;
            score = Mathf.RoundToInt(score);
            GetComponentInChildren<Text>().text = "Score:" + score;
        }
    }

    public int GetScore()
    {
        int c = Mathf.RoundToInt(score);
        return c;
    }
}
