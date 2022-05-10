using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �÷��̾� ü�� �� UI ���� ��ũ��Ʈ
public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Health playerHealth; // �÷��̾� ü�� UI
    [SerializeField]
    private Image totalhealthBar; // ��ü ü�� �� UI
    [SerializeField]
    private Image currenthealthBar; // ���� ü�� �� UI

    void Start()
    {
        // ��ü ü�� ���� �̹����� �� = ���� ü�� / 10�� ������ �ʱ�ȭ
        totalhealthBar.fillAmount = playerHealth.currentHealth / 10;
    }

    void Update()
    {
        // ���� ü�� ���� �̹����� �� = ���� ü�� / 10�� ������ �����Ӹ��� ������Ʈ
        currenthealthBar.fillAmount = playerHealth.currentHealth / 10;  
    }
}
// fillAmount : Image.type �� Image.Type.Filled �� ������ ��� ǥ�õǴ� �̹����� ��
// 0-1 �������� 0�� �ƹ��͵� ǥ�õ��� �ʰ� 1�� ��ü �̹���