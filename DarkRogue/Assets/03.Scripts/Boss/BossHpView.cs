using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���� ü���� �����ִ� ��ũ��Ʈ
public class BossHpView : MonoBehaviour
{
    [SerializeField]
    private Boss boss;
    // canvas���� ���� �����̴� 
    private Slider Hp;

    // ����
    private void Awake()
    {
        // �����̴� ������Ʈ
        Hp = GetComponent<Slider>();

    }

    // �����̴��� value���� ����ؼ� ������Ʈ 
    void Update()
    {
        // �����̴� value�� = ���� ü�� / �ִ�ü��
        Hp.value = boss.CurrentHP / boss.MaxHP;
    }
}
