using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� ������ �����ϴ� ��ũ��Ʈ
public class EnemySpawn : MonoBehaviour
{
    public int maxCount = 4;            // �ִ� �� �� = 4
    public int enemyCount;              // �� ��
    public float spawnTime = 3f;        // �� ���� �ð�
    public float curTime;               // ���� �ð�
    public Transform[] spawnPoints;     // ���� ������ ��ġ�� ��Ƶδ� �迭
    public bool[] isSpawn;              // ���� �����Ǿ����� bool���� ��Ƶ� �迭
    public GameObject enemy;            // �� ������Ʈ

    public static EnemySpawn instance;  // �� ��ũ��Ʈ�� �ν��ͽ�ȭ - ��𼭵� �� �� �ְ�

    // �ν��Ͻ�ȭ �� bool�� �ʱ�ȭ
    private void Start()
    {
        // ���� �����Ǿ����� Ȯ���� bool�迭 ����
        // �� ���� ��ġ�� ��� �迭�� ��� ��ȣ
        isSpawn = new bool[spawnPoints.Length];
        // ���� ��ġ �迭�� �ϳ��ϳ� ȭ��
        for ( int i = 0; i < isSpawn.Length; i++)
        {
            // ���� x
            isSpawn[i] = false;
        }
        // �ν��Ͻ�ȭ 
        instance = this;
    }

    // 
    void Update()
    {
        // ���� ���� �ð��� �� ���� �ð����� ũ�ų� ���� ���� �� ���� �ִ� ���� ���� ������
        if (curTime >= spawnTime && enemyCount < maxCount)
        {
            // 0 ~ ��ġ �迭 �ε����� ���� �� ������
            int x = Random.Range(0, spawnPoints.Length);
            // ���� x ��ġ�� ���� ���� �ʾҴٸ�
            if (!isSpawn[x])
            // �� ���� �޼��� ����
            SpawnEnemy(x);
        }
        curTime += Time.deltaTime;
    }

    // �� ���� �޼���(����)
    public void SpawnEnemy(int ranN)
    {
        // ���� �ð�
        curTime = 0;
        // ���� +
        enemyCount++;
        // ���� �� ���� ��ġ �迭�� �����ϰ� ����
        Instantiate(enemy, spawnPoints[ranN]);
        // �����Ǿ������� true��ȯ
        isSpawn[ranN] = true;
    }
}
