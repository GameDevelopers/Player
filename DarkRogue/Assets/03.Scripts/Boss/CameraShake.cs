using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 메인 카메라 흔들림을 관리하는 스크립트
public class CameraShake : MonoBehaviour
{
    // 흔들림 강도
    public float ShakeAmount;
    // 흔들림 시간
    private float ShakeTime;
    // 메인 카메라가 위치하는 벡터
    private Vector3 initialPos;

    // 진동 시간 메서드
    public void VibrateFormTime(float time)
    {
        // 흔들릴 시간을 time변수로 초기화
        ShakeTime = time;
    }

    // 메인 카메라가 위치할 벡터를 초기화
    void Start()
    {
        // (8,1,-5)의 위치로 변경
        initialPos = new Vector3(8f, 1f, -5f);
    }

    void Update()
    {
        // 만약 흔들림 시간이 0보다 크면
        if (ShakeTime > 0)
        {
            // 반경(ShakeAmount + initialPos) 안의 랜덤 위치 지정
            transform.position = Random.insideUnitSphere * ShakeAmount + initialPos;
            ShakeTime -= Time.deltaTime;
        }
        // 그외
        else
        {
            // 흔들림 시간 = 0
            ShakeTime = 0.0f;
            // 원래 위치로 되돌림
            transform.position = initialPos;
        }
    }
}
