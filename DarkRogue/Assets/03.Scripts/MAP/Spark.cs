using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark : MonoBehaviour
{
    // 장애물 데미지
    public int sparkDamage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Player")
        {
           // 장애물 데미지 만큼 플레이어 체력 감소
            collision.GetComponent<Health>().TakeDamage(sparkDamage);
        }
    }
}