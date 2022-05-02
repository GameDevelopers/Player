using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark : MonoBehaviour
{
    // ��ֹ� ������
    public int sparkDamage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �±װ� Player���
        if (collision.tag == "Player")
        {
            // ��ֹ� ��������ŭ �÷��̾� ü�°���
            collision.GetComponent<Health>().TakeDamage(sparkDamage);
        }
    }
}
