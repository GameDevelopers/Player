using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 플레이어 체력 바 UI 관여 스크립트
public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Health playerHealth; // 플레이어 체력 UI
    [SerializeField]
    private Image totalhealthBar; // 전체 체력 바 UI
    [SerializeField]
    private Image currenthealthBar; // 현재 체력 바 UI

    void Start()
    {
        // 전체 체력 바의 이미지의 양 = 현재 체력 / 10의 값으로 초기화
        totalhealthBar.fillAmount = playerHealth.currentHealth / 10;
    }

    void Update()
    {
        // 현재 체력 바의 이미지의 양 = 현재 체력 / 10의 값으로 프레임마다 업데이트
        currenthealthBar.fillAmount = playerHealth.currentHealth / 10;  
    }
}
// fillAmount : Image.type 이 Image.Type.Filled 로 설정된 경우 표시되는 이미지의 양
// 0-1 범위에서 0은 아무것도 표시되지 않고 1은 전체 이미지