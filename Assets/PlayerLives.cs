using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLives : MonoBehaviour
{
    public static PlayerLives singleton;
    public int lives = 3;
    bool invincible;

    public Text displayScore;
    public Highscores hs;

    public float invunerableTime = 0.5f;
    public float flashrate = 0.05f;
    Renderer[] objectRenderers;

    public GameObject gameOverScreen;
    // Start is called before the first frame update
    public void LoseLife()
    {
        if (!invincible)
        {
            lives -= 1;
            invincible = true;

            if(lives>0)
            gameObject.GetComponent<Playsounds>().PlaySound(2);

            PlayerCanvas.singleton.UpdateLives(lives);
            StartCoroutine(Damaged(invunerableTime, flashrate));
        }

        if(lives < 0)
        {
            GameOver();
        }
    }

    // Update is called once per frame
    IEnumerator Damaged(float time, float intervaltime)
    {
        float startflashtime = intervaltime;
        float elapsedTime = 0f;
        while(elapsedTime < time)
        {
            for (int i = 0; i < objectRenderers.Length; i++)
            {
                objectRenderers[i].enabled = false;
                elapsedTime += Time.deltaTime;
                yield return new WaitForSeconds(startflashtime);
                startflashtime -= startflashtime * 0.5f;
                objectRenderers[i].enabled = true;
                elapsedTime += Time.deltaTime;
                yield return new WaitForSeconds(startflashtime);
            }
        }
        yield return new WaitForSeconds(0.05f);
        invincible = false;
    }

    void GameOver()
    {
        gameObject.GetComponent<Playsounds>().PlaySound(3);
        PlayerCanvas.singleton.gameover = true;
        int score = PlayerCanvas.singleton.GetScore();
        gameOverScreen.SetActive(true);

        displayScore.text = "Score: " + score;
        hs.prepHighScoreScore(score);
        transform.position = new Vector3(50,50,50);
    }

    private void Awake()
    {
        if(singleton)
        {
            Destroy(this);
        }
        else
        {
            singleton = this;
        }
        objectRenderers = GetComponentsInChildren<Renderer>();
        if(gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
    }
}
