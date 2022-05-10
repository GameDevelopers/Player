using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 보스 체력을 보여주는 스크립트
public class BossHpView : MonoBehaviour
{
    [SerializeField]
    private Boss boss;
    // canvas내에 만든 슬라이더 
    private Slider Hp;

    // 참조
    private void Awake()
    {
        // 슬라이더 컴포넌트
        Hp = GetComponent<Slider>();

    }

    // 슬라이더의 value값을 계속해서 업데이트 
    void Update()
    {
        // 슬라이더 value값 = 현재 체력 / 최대체력
        Hp.value = boss.CurrentHP / boss.MaxHP;
    }
}
