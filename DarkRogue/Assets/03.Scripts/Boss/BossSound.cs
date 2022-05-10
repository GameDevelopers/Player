using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 보스 맵에서 죽을 때 사용할 오디오 클립 스크립트
public class BossSound : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip dieSound;

    // 보스 사운드 메서드
    public void PlayBossSound(string action)
    {
        // "action"
        switch (action)
        {
            // 만약 "action"이 "DIE"라면 
            case "DIE":
                // die클립 실행
                audioSource.clip = dieSound;
                break;
        }
        audioSource.Play();
    }
}
