using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public int maxCount = 4;
    public int enemyCount;
    public float spawnTime = 3f;
    public float curTime;
    public Transform[] spawnPoints;
    public bool[] isSpawn;
    public GameObject enemy;

    public static EnemySpawn instance;

    private void Start()
    {
        isSpawn = new bool[spawnPoints.Length];
        for ( int i = 0; i < isSpawn.Length; i++)
        {
            isSpawn[i] = false;
        }
        instance = this;
    }

    void Update()
    {
        if (curTime >= spawnTime && enemyCount < maxCount)
        {
            int x = Random.Range(0, spawnPoints.Length);
            if (!isSpawn[x])
            SpawnEnemy(x);
        }
        curTime += Time.deltaTime;
    }

    public void SpawnEnemy(int ranN)
    {
        curTime = 0;
        enemyCount++;
        Instantiate(enemy, spawnPoints[ranN]);
        isSpawn[ranN] = true;
    }
}
