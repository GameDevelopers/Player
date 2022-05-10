using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �ʿ��� ���� �� ����� ����� Ŭ�� ��ũ��Ʈ
public class BossSound : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip dieSound;

    // ���� ���� �޼���
    public void PlayBossSound(string action)
    {
        // "action"
        switch (action)
        {
            // ���� "action"�� "DIE"��� 
            case "DIE":
                // dieŬ�� ����
                audioSource.clip = dieSound;
                break;
        }
        audioSource.Play();
    }
}
