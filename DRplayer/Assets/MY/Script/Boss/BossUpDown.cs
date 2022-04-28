using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUpDown : StateMachineBehaviour
{
    [SerializeField] private Boss boss;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.AttackUpNDown();
    }
}
