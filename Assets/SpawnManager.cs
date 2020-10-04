using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int DangerPool = 8;
    public GameObject danger;
    public GameObject specialSpawn;
    GameObject[] dangers;
    Transform[] spawnPoints;
    public SpawnPattern [] pats;
    int index = 0;
    static int spawnOptions = 3;
    float wavedelay = 1f;
    int[] RandomMinMax = {2,98};
    int randomNumber = 0;
    int waveCount;
    int[] goodPatterns = { 1, 2, 3, 5, 6, 7, 9, 10, 11, 13 };

    public float baseSpeed = 350f;

    [Header("Random Override")]
    public bool overrider;
    public int overriderNumber;
    int test = 0;
    int speedtestCounter = 0;

    void Awake()
    {
        index = 0;
        if (specialSpawn != null)
        {
            dangers = new GameObject[DangerPool + 1];
        }
        else
        {
            dangers = new GameObject[DangerPool];
        }
        spawnPoints = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints[i] = transform.GetChild(i);
        }


        for (int i = 0; i < DangerPool; i++)
        {
            if (danger != null)
            {
                GameObject d = Instantiate(danger,transform);
                dangers[i] = d;
                d.SetActive(false);
            }
        }

        if(specialSpawn != null)
        {
            GameObject d = Instantiate(specialSpawn);
            dangers[DangerPool] = d;
            d.SetActive(false);
        }
    }

    private void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        randomNumber = RandomNum();
        SpawnPicker(randomNumber);
        //StartCoroutine(SpawnPattern2());
        //StartCoroutine(burstSpawn(pats));
    }

    void SpawnPicker(int randomI)
    {
        waveCount++;
        speedtestCounter++;
        if(speedtestCounter > 10)
        {
            speedtestCounter = 0;
            IncreaseSpeed();
        }
        if (overrider)
        {
            randomI = overriderNumber;
        }
        switch (randomI)
        {
            case 0:
                StartCoroutine(burstSpawn(pats));
                break;

            case 1:
                StartCoroutine(SpawnPattern());
                break;
            case 2:
                StartCoroutine(SpawnPattern2());
                break;

            default:
                StartCoroutine(SpawnPattern2());
                break;
        }
         //StartCoroutine(SpawnPattern2());
         //StartCoroutine(burstSpawn(pats));
    }

    IEnumerator burstSpawn(SpawnPattern[] ps)
    {
        randomNumber = RandomNum();
        for (int i = Random.Range(0,ps.Length); i < ps.Length; i++)
        {
            for (int c = 0; c < ps[i].columns.Length; c++)
            {
                if (ps[i].itemDelay <= 0f)
                {
                    Spawn(ps[i].columns[c].rows);
                }

                else
                {
                    for (int item = 0; item < ps[i].columns[c].rows.Length; item++)
                    {
                        Spawn(ps[i].columns[c].rows[item]);
                        yield return new WaitForSeconds(ps[i].itemDelay);
                    }
                }
                yield return new WaitForSeconds(ps[i].delayTime);
            }
            yield return new WaitForSeconds(ps[i].waveDelay);
            if(Random.Range(0,50) % 2 == 0 )
            {
                SpawnPicker(randomNumber);
                yield break;
            }
        }
        SpawnPicker(randomNumber);
    }

    //burst pattern
    void Spawn(int[] locations )
    {
        for (int i = 0; i < locations.Length; i++)
        {
            dangers[index].transform.position = spawnPoints[locations[i]].position;
            dangers[index].SetActive(true);

            if(index < DangerPool-1)
            {
                index++;
            }
            else
            {
                index = 0;
            }

        }
    }

    void Spawn(int location)
    {
        int ri = Random.Range(0, 100);

        if (ri > 85 - waveCount && specialSpawn != null && !specialSpawn.activeInHierarchy)
        {
            dangers[DangerPool].transform.position = spawnPoints[location].position;
            dangers[DangerPool].SetActive(true);
            waveCount = 0;
            return;
        }

            dangers[index].transform.position = spawnPoints[location].position;
            dangers[index].SetActive(true);

            if (index < DangerPool - 1)
            {
                index++;
            }
            else
            {
                index = 0;
            }
        }

    //funky patterns
    IEnumerator SpawnPattern()
    {
        int pattern = Random.Range(0, goodPatterns.Length);
        int spawntest = Random.Range(8,DangerPool);
        randomNumber = RandomNum();
        //counterclockwisepattern
        for (int i = 0; i < spawntest; i++)
        {
            dangers[index].transform.position = spawnPoints[(i * goodPatterns[pattern]) % spawnPoints.Length].position;

            dangers[index].SetActive(true);

            index = index < DangerPool - 1 ? index + 1 : index - index;

            yield return new WaitForSeconds(0.1f);
        }
        if (spawntest < 16)
        {
            yield return new WaitForSeconds(0.01f);
        }
        else if (spawntest > 16 && spawntest < 33)
        {
            yield return new WaitForSeconds(0.08f);
        }
        else
        {
            yield return new WaitForSeconds(0.6f);
        }
        SpawnPicker(randomNumber);
    }

    IEnumerator SpawnPattern2()
    {
        int pattern = Random.Range(0, goodPatterns.Length);
        int spawntest = Random.Range(8, DangerPool);
        randomNumber = RandomNum();
        //clockwisepattern
        for (int i = spawntest; i > -1 ; i--)
        {
            dangers[index].transform.position = spawnPoints[(i * goodPatterns[pattern]) % spawnPoints.Length].position;
            dangers[index].SetActive(true);

            index = index < DangerPool - 1 ? index + 1 : index - index;

            yield return new WaitForSeconds(0.11f);
        }
        if (spawntest < 16)
        {
            yield return new WaitForSeconds(0.01f);
        }
        else if(spawntest > 16 && spawntest < 33)
        {
            yield return new WaitForSeconds(0.08f);
        }
        else
        {
            yield return new WaitForSeconds(0.6f);
        }

        SpawnPicker(randomNumber);
    }

    int RandomNum()
    {
        int c = Random.Range(RandomMinMax[0], RandomMinMax[1]) % spawnOptions;
        return c;
    }

    void IncreaseSpeed()
    {
        baseSpeed += 100;
    }
}
