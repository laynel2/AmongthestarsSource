using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnPat", menuName = "ScriptableObjects/SpawnPatterns", order = 1)]
public class SpawnPattern : ScriptableObject
{
    //public string[] spawnLocations;
    public float delayTime = 0.3f;
    public static int x,y;
    public float waveDelay = 0.4f;
    public float itemDelay = 0f;

    [System.Serializable]
    public class Column
    {
        public int[] rows = new int[y];
    }

    public Column[] columns = new Column[x];
}
