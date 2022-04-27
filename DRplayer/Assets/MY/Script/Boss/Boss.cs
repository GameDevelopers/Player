using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public enum BossState { Appear = 0, Phase1, Phase2}

public class Boss : MonoBehaviour
{
    [SerializeField]
    private float maxHP = 10;    // ���� �ִ� HP
    private float currentHP;    // ���� ������ HP
    public int bossDamage = 1;    // ���� ���ݷ�
    private SpriteRenderer spriteRenderer;
    public float MaxHP => maxHP;    // HpView ��ũ��Ʈ���� ���� ����.
    public float CurrentHP => currentHP;    // HpView ��ũ��Ʈ���� ���� ����.
    //private Animator animator;
    //private Animation anim;
    [SerializeField]private float bossMoveSpeed;
    [SerializeField]private Vector2 bossMoveDirection;
    [SerializeField]private float attackMoveSpeed;
    [SerializeField]private Vector2 attackMoveDirection;
    [SerializeField]private float attackPlayerSpeed;
    [SerializeField]private Transform player;
    private Vector2 playerPosition;
    [SerializeField] private Transform platformCheckUp;
    [SerializeField] private Transform platformCheckDown;
    [SerializeField] private Transform platformCheckWall;
    [SerializeField] private float platformCheckRadius;
    [SerializeField] private LayerMask platformLayer;
    private bool isTouchingUp;
    private bool isTouchingDown;
    private bool isTouchingWall;
    private bool goingUp = true;
    private bool facingLeft = true;
    private Rigidbody2D bossRB;

    private void Awake()
    {
        // ���� HP�� �ִ� HP��
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        bossMoveDirection.Normalize();
        attackMoveDirection.Normalize();
        bossRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        isTouchingUp = Physics2D.OverlapCircle(platformCheckUp.position, platformCheckRadius, platformLayer);
        isTouchingDown = Physics2D.OverlapCircle(platformCheckDown.position, platformCheckRadius, platformLayer);
        isTouchingWall = Physics2D.OverlapCircle(platformCheckWall.position, platformCheckRadius, platformLayer);
        //BossState();
        AttackUpNDown();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AttackPlayer();
        }
        FlipTowardPlayer();
    }

    private void BossState()
    {
        if ( isTouchingUp && goingUp )
        {
            ChangeDirection();
        }
        else if ( isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }
        if ( isTouchingWall)
        {
            if (facingLeft)
            {
                Flip();
            }
            else if (!facingLeft)
            {
                Flip();
            }
        }
        bossRB.velocity = bossMoveSpeed * bossMoveDirection;
    }

    private void AttackUpNDown()
    {
        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
        }
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }
        if (isTouchingWall)
        {
            if (facingLeft)
            {
                Flip();
            }
            else if (!facingLeft)
            {
                Flip();
            }
        }
        bossRB.velocity = attackMoveSpeed * attackMoveDirection;
    }

    private void AttackPlayer()
    {
        // �÷��̾� ��ǥ
        playerPosition = player.position - transform.position;
        // �÷��̾� ��ǥ �븻������
        playerPosition.Normalize();
        // �� ��ǥ�� ����
        bossRB.velocity = playerPosition * attackPlayerSpeed;
    }

    private void FlipTowardPlayer()
    {
        float playerDirection = player.position.x - transform.position.x;
        if (playerDirection > 0 && facingLeft)
        {
            Flip();
        }
        else if (playerDirection < 0 && !facingLeft)
        {
            Flip();
        }
    }

    private void ChangeDirection()
    {
        goingUp = !goingUp;
        bossMoveDirection.y *= -1;
        attackMoveDirection.y *= -1;
    }

    private void Flip()
    {
        facingLeft = !facingLeft;
        bossMoveDirection.x *= -1;
        attackMoveDirection.x *= -1;
        transform.Rotate(0, 180, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(platformCheckUp.position, platformCheckRadius);
        Gizmos.DrawWireSphere(platformCheckDown.position, platformCheckRadius);
        Gizmos.DrawWireSphere(platformCheckWall.position, platformCheckRadius);
    }
    // ������ �Դ� ������
    public void BossDamaged(float damage)
    {
        //if (isBossDie) return;
        //Debug.Log(isBossDie);
        // ���� ���� hp�� 0���� �۰ų� ���ٸ�
        if (currentHP == 0)
        {
            // ü���� 0�̸� ���� ���.
            BossDie();
            //isBossDie = true;
        }

        // ���� HP�� -damage��ŭ
        currentHP -= damage;

        StopCoroutine("HitAnimation");
        StartCoroutine("HitAnimation");


    }

    //public void ChangeState(BossState newState)
    //{
    //    StopCoroutine(bossState.ToString());
    //    bossState = newState;
    //    StartCoroutine(bossState.ToString());
    //}


    //private IEnumerator Appear()
    //{
    //    movement.Move(Vector3.up);

    //    while (true)
    //    {
    //        if (transform.position.y <= bossAppear)
    //        {
    //            //movement.Move(Vector3.zero);
    //            ChangeState(BossState.Phase1);
    //        }
    //        yield return null;
    //    }

    //}

    //private IEnumerator Phase1()
    //{

    //    yield return new WaitForSeconds(0.2f);
    //    animator.SetTrigger("IsAttack1");

    //    yield return new WaitForSeconds(0.3f);
    //    bossWeapon.StartFire(AtkType.Circle);
    //    while (true)
    //    { 
    //        if (CurrentHP <= MaxHP * 0.5f)
    //        {
    //            animator.SetTrigger("IsRun");
    //            ChangeState(BossState.Phase2);
    //        }
    //        yield return null;
    //    }
    //}

    //private IEnumerator Phase2()
    //{
    //    //yield return new WaitForSeconds(1.0f);
    //    animator.SetTrigger("IsAttack2");
    //    yield return null;

    //}

    // �÷��̾�� �浹�ϸ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // �� ���ݷ¸�ŭ �÷��̾� ü�°���
            collision.GetComponent<Health>().TakeDamage(bossDamage);
        }
    }

    private IEnumerator HitAnimation()
    {
        // ���� ����
        spriteRenderer.color = Color.black;
        //0.1�� ���
        yield return new WaitForSeconds(0.1f);
        // ���� ��������
        spriteRenderer.color = Color.white;
    }

    public void BossDie()
    {
        //isBossDie = true;
        //// ���� �ı� ����Ʈ ����
        ////BossClearText.SetActive(true);
        ////Vector3 pos;
        ////pos = this.gameObject.transform.position;
        //animator.SetTrigger("IsDead");
        ////Instantiate(bossDiePrefab, transform.position, Quaternion.identity);
        //yield return new WaitForSeconds(1.0f);
        //// ���� ������Ʈ ����
        Destroy(gameObject);
    }
}
