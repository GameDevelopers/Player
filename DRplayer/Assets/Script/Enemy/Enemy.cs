using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 적 체력
    public int hp = 3;
    // 적 공격력
    public int enemyDamage = 1;
    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (hp <= 0)
        {
            EnemySpawn.instance.enemyCount--;
            EnemySpawn.instance.isSpawn[int.Parse(transform.parent.name) - 1] = false;
            Destroy(this.gameObject);
        }
    }

    public void HitDamage(int damage)
    {
        hp = hp - damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 적 공격력만큼 플레이어 체력감소
           playerController.DamageHit(enemyDamage);
        }
    }
}
