using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Animator ���� �ӽ��� ���� ��ȭ(BossAttackPlyer)�� �°� �Լ��� ����
public class BossAttackPlyer : StateMachineBehaviour
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
        // �÷��̾� ���� �޼��� ����
        boss.AttackPlayer();
    }
}
