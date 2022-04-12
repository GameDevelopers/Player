using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackManager : MonoBehaviour
{
    //public GameObject meleeWeaponPrefab;

    // 근접 공격이 EnemyHealth 스크립트가 있는 게임 오브젝트와 충돌할 때 
    // 플레이어가 아래쪽 또는 수평으로 얼마나 움직여야 하는지
    public float defaultForce = 300;
    // 플레이어가 위쪽으로 얼마나 움직여야 하는지
    public float upwardsForce = 600;
    // 플레이어가 이동해야 하는 시간
    public float movementTime = .1f;
    //근접 공격을 수행하는 버튼이 눌렸는지 확인하는 입력 감지
    private bool meleeAttack;
    //meleePrefab의 애니메이터
    [SerializeField]
    private Animator meleeAnimator;
    
    private PlayerController playerController;
    [SerializeField]
    private Animator animator;

    private void Start()
    {
        // 플레이어의 애니메이터 구성 요소
        animator = GetComponent<Animator>();
        //The Character script on the player; this script on my project manages the grounded state, so if you have a different script for that reference that script
        playerController = GetComponent<PlayerController>();
        //The animator on the meleePrefab
        meleeAnimator = GetComponent<MeleeWeapon>().gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        //Method that checks to see what keys are being pressed
        CheckInput();
    }

    private void CheckInput()
    {
        //Checks to see if Backspace key is pressed which I define as melee attack; you can of course change this to anything you would want
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //Sets the meleeAttack bool to true
            meleeAttack = true;
        }
        else
        {
            //Turns off the meleeAttack bool
            meleeAttack = false;
        }
        //Checks to see if meleeAttack is true and pressing up
        if (meleeAttack && Input.GetAxis("Vertical") > 0)
        {
            //Turns on the animation for the player to perform an upward melee attack
            animator.SetTrigger("UpwardMelee");
            Debug.Log("가져옴");
            //Turns on the animation on the melee weapon to show the swipe area for the melee attack upwards
            meleeAnimator.SetTrigger("UpwardMeleeSwipe");
            Debug.Log("공격위가져옴");
        }
        //Checks to see if meleeAttack is true and pressing down while also not grounded
        if (meleeAttack && Input.GetAxis("Vertical") < 0 && !playerController.isGround)
        {
            //Turns on the animation for the player to perform a downward melee attack
            animator.SetTrigger("DownwardMelee");
            Debug.Log("가져옴");
            //Turns on the animation on the melee weapon to show the swipe area for the melee attack downwards
            meleeAnimator.SetTrigger("DownwardMeleeSwipe");
            Debug.Log("공격아래가져옴");
        }
        //Checks to see if meleeAttack is true and not pressing any direction
        if ((meleeAttack && Input.GetAxis("Vertical") == 0)
            //OR if melee attack is true and pressing down while grounded
            || meleeAttack && (Input.GetAxis("Vertical") < 0 ) && playerController.isGround)
        {
            //Turns on the animation for the player to perform a forward melee attack
            animator.SetTrigger("ForwardMelee");
            Debug.Log("가져옴");
            //Turns on the animation on the melee weapon to show the swipe area for the melee attack forwards
            meleeAnimator.SetTrigger("ForwardMeleeSwipe");
            Debug.Log("공격전면가져옴");
        }
    }
}
