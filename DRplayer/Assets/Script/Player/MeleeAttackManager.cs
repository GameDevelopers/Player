using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackManager : MonoBehaviour
{
    // ���� ������ EnemyHealth ��ũ��Ʈ�� �ִ� ���� ������Ʈ�� �浹�� �� �÷��̾ �Ʒ��� �Ǵ� �������� �󸶳� �������� �ϴ���
    public float defaultForce = 300;
    // ���� ������ EnemyHealth ��ũ��Ʈ�� �ִ� ���� ������Ʈ�� �浹�� �� �÷��̾ �������� �󸶳� �������� �ϴ���
    public float upwardsForce = 600;
    // ���� ������ EnemyHealth ��ũ��Ʈ�� �ִ� ���� ������Ʈ�� �浹�� �� �÷��̾ �̵��ؾ� �ϴ� �ð�
    public float movementTime = .1f;
    //���� ������ �����ϴ� ��ư�� ���ȴ��� Ȯ���ϴ� �Է� ����
    private bool meleeAttack;
    //meleePrefab�� �ִϸ�����
    private Animator meleeAnimator;

    private PlayerController playerController;
    private Animator animator;

    private void Start()
    {
        //The Animator component on the player
        animator = GetComponent<Animator>();
        //The Character script on the player; this script on my project manages the grounded state, so if you have a different script for that reference that script
        playerController = GetComponent<PlayerController>();
        //The animator on the meleePrefab
        meleeAnimator = GetComponentInChildren<MeleeWeapon>().gameObject.GetComponent<Animator>();
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
            //Turns on the animation on the melee weapon to show the swipe area for the melee attack upwards
            meleeAnimator.SetTrigger("UpwardMeleeSwipe");
        }
        //Checks to see if meleeAttack is true and pressing down while also not grounded
        if (meleeAttack && Input.GetAxis("Vertical") < 0 && !playerController.isGround)
        {
            //Turns on the animation for the player to perform a downward melee attack
            animator.SetTrigger("DownwardMelee");
            //Turns on the animation on the melee weapon to show the swipe area for the melee attack downwards
            meleeAnimator.SetTrigger("DownwardMeleeSwipe");
        }
        //Checks to see if meleeAttack is true and not pressing any direction
        if ((meleeAttack && Input.GetAxis("Vertical") == 0)
            //OR if melee attack is true and pressing down while grounded
            || meleeAttack && (Input.GetAxis("Vertical") < 0 ) && playerController.isGround)
        {
            //Turns on the animation for the player to perform a forward melee attack
            animator.SetTrigger("ForwardMelee");
            //Turns on the animation on the melee weapon to show the swipe area for the melee attack forwards
            meleeAnimator.SetTrigger("MeleeSwipe");
        }
    }
}
