using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Animator ���� �ӽ��� ���� ��ȭ(BossIdle)�� �°� �Լ��� ����
public class BossIdle : StateMachineBehaviour
{
    [SerializeField] private Boss boss;

    // ���ο� ���·� ���� �� ����
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ������� �±׸� ���� ������Ʈ���� ������Ʈ ������.
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
    }

    // ó���� ������ �������� ������ �� ������ ������ ����
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ���� ��ũ��Ʈ �� ���� �޼��� ����
        boss.BossState();
    }
}
