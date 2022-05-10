using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적 생성에 관여하는 스크립트
public class EnemySpawn : MonoBehaviour
{
    public int maxCount = 4;            // 최대 적 수 = 4
    public int enemyCount;              // 적 수
    public float spawnTime = 3f;        // 적 생성 시간
    public float curTime;               // 현재 시간
    public Transform[] spawnPoints;     // 적을 생성할 위치를 담아두는 배열
    public bool[] isSpawn;              // 적이 생성되었는지 bool값을 담아둘 배열
    public GameObject enemy;            // 적 오브젝트

    public static EnemySpawn instance;  // 이 스크립트를 인스터스화 - 어디서든 쓸 수 있게

    // 인스턴스화 및 bool값 초기화
    private void Start()
    {
        // 적이 생성되었는지 확인할 bool배열 값은
        // 적 생성 위치가 담긴 배열의 요소 번호
        isSpawn = new bool[spawnPoints.Length];
        // 생성 위치 배열을 하나하나 화인
        for ( int i = 0; i < isSpawn.Length; i++)
        {
            // 생성 x
            isSpawn[i] = false;
        }
        // 인스턴스화 
        instance = this;
    }

    // 
    void Update()
    {
        // 만약 현재 시간이 적 생성 시간보다 크거나 같고 현재 적 수가 최대 적수 보다 적으면
        if (curTime >= spawnTime && enemyCount < maxCount)
        {
            // 0 ~ 위치 배열 인덱스를 랜덤 및 변수로
            int x = Random.Range(0, spawnPoints.Length);
            // 만약 x 위치에 생성 되지 않았다면
            if (!isSpawn[x])
            // 적 생성 메서드 실행
            SpawnEnemy(x);
        }
        curTime += Time.deltaTime;
    }

    // 적 생성 메서드(랜덤)
    public void SpawnEnemy(int ranN)
    {
        // 현재 시간
        curTime = 0;
        // 적수 +
        enemyCount++;
        // 적을 적 생성 위치 배열에 랜덤하게 생성
        Instantiate(enemy, spawnPoints[ranN]);
        // 생성되었는지를 true반환
        isSpawn[ranN] = true;
    }
}
