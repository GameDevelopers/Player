using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Animator 상태 머신의 상태 변화(BossIdle)에 맞게 함수를 실행
public class BossIdle : StateMachineBehaviour
{
    [SerializeField] private Boss boss;

    // 새로운 상태로 변할 때 실행
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 보스라는 태그를 가진 오브젝트에서 컴포넌트 가져옴.
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
    }

    // 처음과 마지막 프레임을 제외한 각 프레임 단위로 실행
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 보스 스크립트 내 상태 메서드 실행
        boss.BossState();
    }
}
