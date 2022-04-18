using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // ü�� ����
    public int health;
    // ��Ʈ�� �� ����
    public int numOfHearts;

    // ü�� �̹��� �迭
    public Image[] hearts;
    // ���ִ� ü�� ��������Ʈ
    public Sprite heartFull;
    // �� ü�� ��������Ʈ
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
        // ���� �÷��̾ ������ �ִ� ��Ʈ ������ ������ Ȯ��
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
