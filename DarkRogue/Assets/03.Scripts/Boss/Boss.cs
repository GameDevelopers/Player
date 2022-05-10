using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    // ���� ü��
    [Header("Boss HP")]
    [SerializeField]
    private float maxHP = 10;    // ���� �ִ� HP
    private float currentHP;    // ���� ������ HP
    public int bossDamage = 1;    // ���� ���ݷ�
    private SpriteRenderer spriteRenderer;
    public float MaxHP => maxHP;    // HpView ��ũ��Ʈ���� ���� ����.
    public float CurrentHP => currentHP;    // HpView ��ũ��Ʈ���� ���� ����.

    [Header("BossMove")]
    [SerializeField] private float bossMoveSpeed; // ������ ������ �ӵ�
    [SerializeField] private Vector2 bossMoveDirection; // ���� ������ ����

    [Header("AttackUp&Down")]
    [SerializeField] private float attackMoveSpeed; // ���� �� �����̴� �ӵ�
    [SerializeField] private Vector2 attackMoveDirection; // ���� �� �����̴� ����

    [Header("AttackPlayer")]
    [SerializeField] private float attackPlayerSpeed; // ���� �� �÷��̾�� ���ϴ� �ӵ� 
    [SerializeField] private GameObject player; // �÷��̾��� ��ġ���� �������� ���� ������Ʈ
    [SerializeField]
    private Vector3 playerPosition; // �÷��̾� ��ġ
    [SerializeField]
    private bool isPlayerPosition; // �÷��̾� ��ġ�� �����°�?

    [Header("Others")]
    [SerializeField] private Transform platformCheckUp; // �� �� üũ
    [SerializeField] private Transform platformCheckDown; // �� �Ʒ� üũ
    [SerializeField] private Transform platformCheckWall; // �� üũ
    [SerializeField] private float platformCheckRadius; // ����ĳ��Ʈ �� �÷����� üũ�ϱ� ���� �ݰ�
    [SerializeField] private LayerMask platformLayer; // �÷��� ���̾�
    private bool isTouchingUp; // ���� ��Ҵ°�
    private bool isTouchingDown; // �Ʒ� ��Ҵ°�
    private bool isTouchingWall; // ���� ��Ҵ°�
    private bool goingUp = true; // �ö󰡴� ���� bool
    private bool facingLeft = true; // ���ʺ��� ���� bool
    private Rigidbody2D bossRB;
    private Animator bossAnimation;
    // ������� ����(�����Ǳ� �� ����)
    [SerializeField]
    private BossMapBGM bossBGM;
    [SerializeField]
    private GameObject nextScene;
    [SerializeField]
    private GameObject dieParticle;
    // ������ �� �� �ݶ��̴��� ����� �� ī�޶� ��鸲 ����.
    [SerializeField]
    private CameraShake Camera;

    // ���� �ҷ����� �� �ʱ�ȭ
    private void Awake()
    {
        // ���� HP�� �ִ� HP��
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        bossMoveDirection.Normalize();
        attackMoveDirection.Normalize();
        bossRB = GetComponent<Rigidbody2D>();
        bossAnimation = GetComponent<Animator>();
        Camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
    }

    // ������ �� ���� ��, �Ʒ�, �� üũ
    private void Update()
    {
        // �ش� ���̾� ����ũ�� ������ ���� ������Ʈ�� ���� ���θ� �˻��Ͽ�, �߽߰� ��(True) ���� ��ȯ
        // ��ǥ, ������, ���̾�
        isTouchingUp = Physics2D.OverlapCircle(platformCheckUp.position, platformCheckRadius, platformLayer);
        isTouchingDown = Physics2D.OverlapCircle(platformCheckDown.position, platformCheckRadius, platformLayer);
        isTouchingWall = Physics2D.OverlapCircle(platformCheckWall.position, platformCheckRadius, platformLayer);
        AttackPlayer();
    }

    // ���¸ӽ� ������ �ش� ���ϸ��̼� ������ �������� �ֱ� ���� = �� �� ���� ���� 
    private void randomStatePick()
    {
        // 0�� 1 �� �� ���� ��
        int randomState = Random.Range(0, 2);
        // ���� 0�̸�,
        if (randomState == 0)
        {
            // AttackUPDown �ִϸ��̼�
            bossAnimation.SetTrigger("AttackUpDown");
        }
        // 1�̸�,
        else if (randomState == 1)
        {
            // AttackPlayer �ִϸ��̼�
            bossAnimation.SetTrigger("AttackPlayer");
        }
    }

    // ���� ���� �޼���
    public void BossState()
    {
        // ���� ���ʿ� ��Ұ� �ö󰡴� ���̶��
        if (isTouchingUp && goingUp)
        {
            // �� �޼��� ����
            ChangeDirection();
        }
        // �Ʒ� ��Ұ� �ö��� �ʴ´ٸ�
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }
        // ���� ��Ҵٸ�
        if (isTouchingWall)
        {
            // ������ ���� �ִٸ�
            if (facingLeft)
            {
                // ��������Ʈ ����
                Flip();
            }
            // �������� ���� �ִٸ�
            else if (!facingLeft)
            {
                Flip();
            }
        }
        // ������ �ӵ� = �ӵ� * ����
        bossRB.velocity = bossMoveSpeed * bossMoveDirection;
    }

    // �� �Ʒ� ���� �޼���
    public void AttackUpNDown()
    {
        // ���� ���� ��Ұ� �ö󰡴� ���̶��
        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
        }
        // �Ʒ� ��Ұ� �ö󰡴� ���� �ƴ϶��
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }
        // ���� ��Ұ� ������ ����
        if (isTouchingWall && facingLeft)
        {
            Flip();
        }
        // ���� ��Ұ� �������� ���� ������
        else if (!facingLeft && isTouchingWall)
        {
            Flip();
        }
        // ���� �ӵ� = ���� �� �ӵ� * ���� �� ����
        bossRB.velocity = attackMoveSpeed * attackMoveDirection;
    }

    // �÷��̾� ���ϴ� ���� �޼���
    public void AttackPlayer()
    {
        // ���� �÷��̾� ��ġ���� ���ٸ�
        if (!isPlayerPosition)
        {
            // �÷��̾� ��ǥ
            playerPosition = player.transform.position + transform.position;
            // �÷��̾ ���� ��������Ʈ�� �����ϴ� �޼���
            FlipTowardPlayer();
            //�÷��̾� ��ǥ ��ֶ�����
            playerPosition.Normalize();
            // �÷��̾� ��ġ�� �ʱ�ȭ
            isPlayerPosition = true;
        }
        // ��ġ���� �����Ѵٸ�
        if (isPlayerPosition)
        {
            //�� ��ǥ�� ����(�÷��̾� ��ġ ���� * �÷��̾ ���� �ӵ�)
            bossRB.velocity = playerPosition * attackPlayerSpeed;
        }
        // ���� ��Ұų� �Ʒ� ������
        if (isTouchingWall || isTouchingDown)
        {
            // �÷��̾� ��ġ�� �ʱ�ȭ
            isPlayerPosition = false;
            // ������ �ӵ��� 0���� ���� �� ���� �ִϸ��̼�
            bossRB.velocity = Vector2.zero;
            bossAnimation.SetTrigger("Stuned");
        }
    }

    // �÷��̾ ������ ����ġ�� ���� �� �ְ� ��������Ʈ�� �������ִ� �޼���
    private void FlipTowardPlayer()
    {
        // �÷��̾� ����
        float playerDirection = player.transform.position.x - transform.position.x;
        // �÷��̾� ������ ���(����)�̰� ������ ������ �ٶ󺸰� �ִٸ�
        if (playerDirection > 0 && facingLeft)
        {
            // ��������Ʈ ����.
            Flip();
        }
        // �÷��̾� ������ ����(����)�̰� ������ �������� �ٶ󺸰� �ִٸ�
        else if (playerDirection < 0 && !facingLeft)
        {
            Flip();
        }
    }

    // ������ ��ȯ���ִ� �޼���
    private void ChangeDirection()
    {
        // �ö󰡴� ���¸� �ö��� �ʴ� ���·� ����
        goingUp = !goingUp;
        // ������ y�������� �ݴ��
        bossMoveDirection.y *= -1;
        // ���ݽ� ������ �������� �ݴ��
        attackMoveDirection.y *= -1;
        // ���� ī�޶� ���� �ð�
        Camera.VibrateFormTime(0.05f);
    }

    // ������ ��������Ʈ�� ������Ű�� �޼���
    private void Flip()
    {
        facingLeft = !facingLeft;
        // ������ �����̴� ������ �ݴ��
        bossMoveDirection.x *= -1;
        // ���� �� ������ �����ӹ����� �ݴ��
        attackMoveDirection.x *= -1;
        // ������Ʈ�� transform�� Rotate���� 180���� �־� ��������Ʈ ����
        transform.Rotate(0, 180, 0);
    }

    // ����� - �ν����� �信�� ������ ���� ��, �Ʒ�, ���� ��Ҵ��� ��Ȳ üũ
    private void OnDrawGizmosSelected()
    {
        // ���� ����
        Gizmos.color = Color.cyan;
        // ��ü�� 
        Gizmos.DrawWireSphere(platformCheckUp.position, platformCheckRadius);
        Gizmos.DrawWireSphere(platformCheckDown.position, platformCheckRadius);
        Gizmos.DrawWireSphere(platformCheckWall.position, platformCheckRadius);
    }

    // ������ �Դ� ������
    public void BossDamaged(float damage)
    {
        // ���� ���� ü���� 2f���
        if (currentHP == 2f)
        {
            // bossScene�� ������ ����
            bossBGM.ChangeBgm(BGMType.BossHpHalf);
        }
        // ���� ���� hp�� 0���� �۰ų� ���ٸ�
        if (currentHP == 0)
        {
            // ü���� 0�̸� ���� ���.
            BossDie();
        }
        // ���� HP�� -damage��ŭ
        currentHP -= damage;
        // �Ʒ� �ۼ��ص� ���� ���� �ڷ�ƾ ���� �� ����
        StopCoroutine("HitAnimation");
        StartCoroutine("HitAnimation");
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

    // ���� �ǰ� �� ���� ���� �ڷ�ƾ
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
        // ���� ������Ʈ ����
        Destroy(gameObject);
        // die ��ƼŬ ������Ʈ on
        dieParticle.SetActive(true);
        // ���������� ���� ��ũ��Ʈ�� �־�� ������Ʈ on
        nextScene.SetActive(true);
    }
}
