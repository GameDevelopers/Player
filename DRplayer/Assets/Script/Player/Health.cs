using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // 체력 변수
    public int health;
    // 하트의 수 지정
    public int numOfHearts;

    // 체력 이미지 배열
    public Image[] hearts;
    // 차있는 체력 스프라이트
    public Sprite heartFull;
    // 빈 체력 스프라이트
    public Sprite heartEmpty;

    private Enemy enemy;
    private PlayerController playerController;
    private Animator animator;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 현재 플레이어가 가지고 있는 하트 수보다 작은지 확인
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = heartFull;
            }
            else
            {
                hearts[i].sprite = heartEmpty;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    
}
