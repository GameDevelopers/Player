using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// enum함수를 이용해서 아이템 타입 열거
public enum ItemType { GEO, HP }

// 적 처치시 드랍되는 아이템의 속성에 관여하는 스크립트
public class Item : MonoBehaviour
{
    [SerializeField]
    private ItemType itemType; // 아이템 타입 변수
    private float hp = 1f;  // 체력아이템에 필요한 변수

    // 아이템 드랍 시 스플래쉬(퍼짐)
    public Transform itemTransform; // 아이템들의 위치값
    private float delay = 0; 
    private float pasttime = 0;
    private float when = 1.0f;
    private Vector3 off; // 아이템들이 흩어질 방향 벡터

    // 방향 벡터 초기화
    private void Awake()
    {
        // x방향 랜덤
        off = new Vector3(Random.Range(-2, 2), off.y, off.z);
        // y방향 랜덤
        off = new Vector3(off.x, Random.Range(-0.7f, -0.5f), off.z);
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

    // 플레이어와 충돌할 때
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 아이템 타입이 HP라면
        if (itemType == ItemType.HP)
        {
            // 부딫힌 콜라이더가 플레이어 태그를 가졌다면
            if (collision.tag == "Player")
            {
                // 태그 내 Health 컴포넌트의 체력 추가(1) 메서드를 실행
                collision.GetComponent<Health>().AddHealth(hp);
                // 아이템 파괴
                Destroy(gameObject);
            }
        }
        // 아이템 타입이 GEO라면
        else if (itemType == ItemType.GEO)
        {
            // 부딫힌 콜라이더가 플레이어 태그를 가졌다면 
            if (collision.tag == "Player")
            {
                // 아이템 파괴
                Destroy(gameObject);
            }
        }
    }
}
