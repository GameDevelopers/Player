using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { GEO, HP }
public class Item : MonoBehaviour
{
    [SerializeField]
    private ItemType itemType;
    // 체력
    private float hp = 1f;

    // 지오 랜덤 스플래쉬
    public Transform itemTransform;
    private float delay = 0;
    private float pasttime = 0;
    private float when = 1.0f;
    private Vector3 off;

    private void Awake()
    {
        // x방향 랜덤
        off = new Vector3(Random.Range(-2, 2), off.y, off.z);
        // y방향 랜덤
        off = new Vector3(off.x, Random.Range(0, 2), off.z);
    }

    private void Update()
    {
        if(when >= delay)
        {
            pasttime = Time.deltaTime;
            itemTransform.position += off * Time.deltaTime;
            delay += pasttime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (itemType == ItemType.HP)
        {
            if (collision.tag == "Player")
            {
                collision.GetComponent<Health>().AddHealth(hp);
                Destroy(gameObject);
            }
        }
        else if (itemType == ItemType.GEO)
        {
            if (collision.tag == "Player")
            {
                Destroy(gameObject);
            }
        }
    }
}
