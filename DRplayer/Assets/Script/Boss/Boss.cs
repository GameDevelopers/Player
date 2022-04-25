using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BossState { Appear = 0, Phase1, Phase2}

public class Boss : MonoBehaviour
{
    // ������ �׾��°�
    public bool isBossDie = false;
    // ������ ��Ÿ���� �ð�
    [SerializeField]
    private float bossAppear = 2.5f;
    // ���� ���� - ��Ÿ����
    private BossState bossState = BossState.Appear;
    // ������(������ ����)
    private Movement movement;
    private BossWeapon bossWeapon;
    // ���� ���ݷ�
    public int bossDamage = 1;
    // ���� �ִ� HP
    [SerializeField]
    private float maxHP = 10;
    // ���� ������ HP
    private float currentHP;
    //private SpriteRenderer spriteRenderer;

    // HpView ��ũ��Ʈ���� ���� ����.
    public float MaxHP => maxHP;
    // HpView ��ũ��Ʈ���� ���� ����.
    public float CurrentHP => currentHP;
    private Animator animator;
    private Animation anim;


    //[SerializeField]
    //public GameObject BossClearText;

    private void Awake()
    {
        // ����
        movement = GetComponent<Movement>();
        anim = GetComponent<Animation>();
        animator = GetComponent<Animator>();
        bossWeapon = GetComponent<BossWeapon>();
        // ���� HP�� �ִ� HP��
        currentHP = maxHP;
    }

    //public void Update()
    //{
    //    StartCoroutine(Move());
    //}

    // ������ �Դ� ������
    public void BossDamaged(float damage)
    {
        // ���� HP�� -damage��ŭ
        currentHP -= damage;

        //StopCoroutine("HitAnimation");
        //StartCoroutine("HitAnimation");

        // ���� ���� hp�� 0���� �۰ų� ���ٸ�
        if (currentHP <= 0)
        {
            // ü���� 0�̸� ���� ���.
            StartCoroutine("BossDie");
        }
    }

    public void ChangeState(BossState newState)
    {
        StopCoroutine(bossState.ToString());
        bossState = newState;
        StartCoroutine(bossState.ToString());
    }


    private IEnumerator Appear()
    {
        movement.Move(Vector3.up);

        while (true)
        {
            if (transform.position.y <= bossAppear)
            {
                movement.Move(Vector3.zero);
                ChangeState(BossState.Phase1);
            }
            yield return null;
        }

    }

    private IEnumerator Phase1()
    {

        yield return new WaitForSeconds(1.0f);
        animator.SetTrigger("IsAttack1");

        yield return new WaitForSeconds(1.0f);
        bossWeapon.StartFire(AtkType.Circle);
        while (true)
        { 
            if (CurrentHP <= MaxHP * 0.5f)
            {
                animator.SetTrigger("IsRun");
                ChangeState(BossState.Phase2);
            }
            yield return null;
        }
    }

    private IEnumerator Phase2()
    {
        //yield return new WaitForSeconds(1.0f);
        animator.SetTrigger("IsAttack2");
        yield return null;

    }

    // �÷��̾�� �浹�ϸ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // �� ���ݷ¸�ŭ �÷��̾� ü�°���
            collision.GetComponent<Health>().TakeDamage(bossDamage);
        }
    }

    public IEnumerator BossDie()
    {
        isBossDie = true;
        // ���� �ı� ����Ʈ ����
        //BossClearText.SetActive(true);

        yield return new WaitForSeconds(1.0f);


        animator.SetTrigger("IsDead");

        yield return new WaitForSeconds(1.0f);
        // ���� ������Ʈ ����
        Destroy(gameObject);
    }
}
