using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// enum함수로 음악 타입을 index로 열거
public enum BGMType { BossHpFull = 0, BossHpHalf }

//보스 맵에 나오는 음악 관련 스크립트
public class BossMapBGM : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] bgmClips; // 음악 클립을 담을 배열
    private AudioSource audioSource;
    
    // 참조 불러오기
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // 음악 바꿔주는 메서드
    public void ChangeBgm(BGMType index)
    {
        // 현재 재생 중인 배경음악 정지
        audioSource.Stop();
        // 배경음악 목록에서 Index번째의 음악으로 교체
        audioSource.clip = bgmClips[(int)index];
        // 교체된 음악 플레이.
        audioSource.Play();
    }
}
