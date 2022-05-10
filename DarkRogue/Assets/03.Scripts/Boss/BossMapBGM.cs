using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// enum�Լ��� ���� Ÿ���� index�� ����
public enum BGMType { BossHpFull = 0, BossHpHalf }

//���� �ʿ� ������ ���� ���� ��ũ��Ʈ
public class BossMapBGM : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] bgmClips; // ���� Ŭ���� ���� �迭
    private AudioSource audioSource;
    
    // ���� �ҷ�����
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // ���� �ٲ��ִ� �޼���
    public void ChangeBgm(BGMType index)
    {
        // ���� ��� ���� ������� ����
        audioSource.Stop();
        // ������� ��Ͽ��� Index��°�� �������� ��ü
        audioSource.clip = bgmClips[(int)index];
        // ��ü�� ���� �÷���.
        audioSource.Play();
    }
}
