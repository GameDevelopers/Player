using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [Header("Boss HP")]
    [SerializeField]
    private float maxHP = 10;    // ���� �ִ� HP
    private float currentHP;    // ���� ������ HP
    public int bossDamage = 1;    // ���� ���ݷ�
    private SpriteRenderer spriteRenderer;
    public float MaxHP => maxHP;    // HpView ��ũ��Ʈ���� ���� ����.
    public float CurrentHP => currentHP;    // HpView ��ũ��Ʈ���� ���� ����.
                                            
    [Header("BossMove")]
    [SerializeField]private float bossMoveSpeed;
    [SerializeField]private Vector2 bossMoveDirection;
    [Header("AttackUp&Down")]
    [SerializeField]private float attackMoveSpeed;
    [SerializeField]private Vector2 attackMoveDirection;
    [Header("AttackPlayer")]
    [SerializeField]private float attackPlayerSpeed;
    [SerializeField]private GameObject player;
    [SerializeField]
    private Vector3 playerPosition;
    [SerializeField]
    private bool isPlayerPosition;
    [Header("Others")]
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
    private Animator bossAnimation;
    // ������� ����(�����Ǳ� �� ����)
    [SerializeField]
    private BossMapBGM bossBGM;
    [SerializeField]
    private GameObject nextScene;
    [SerializeField]
    private GameObject dieParticle;
    [SerializeField]
    private CameraShake Camera;


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

    private void Update()
    {
        isTouchingUp = Physics2D.OverlapCircle(platformCheckUp.position, platformCheckRadius, platformLayer);
        isTouchingDown = Physics2D.OverlapCircle(platformCheckDown.position, platformCheckRadius, platformLayer);
        isTouchingWall = Physics2D.OverlapCircle(platformCheckWall.position, platformCheckRadius, platformLayer);
        AttackPlayer();
    }

    private void randomStatePick()
    {
        int randomState = Random.Range(0, 2);
        if (randomState == 0)
        {
            // AttackUPDown �ִϸ��̼�
            bossAnimation.SetTrigger("AttackUpDown");
        }
        else if (randomState == 1)
        {
            bossAnimation.SetTrigger("AttackPlayer");
        }
    }

    public void BossState()
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

    public void AttackUpNDown()
    {
        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
            Debug.Log("u");
        }
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
            Debug.Log("d");
        }
        if (isTouchingWall && facingLeft)
        {
            Flip();
            Debug.Log("wl");
        }
        else if (!facingLeft && isTouchingWall)
        {
            Flip();
            Debug.Log("wr");
        }
        bossRB.velocity = attackMoveSpeed * attackMoveDirection;
    }

    public void AttackPlayer()
    {
        if (!isPlayerPosition)
        {
            //�÷��̾� ��ǥ
            playerPosition = player.transform.position + transform.position;
            FlipTowardPlayer();
            //�÷��̾� ��ǥ ��ֶ�����
            playerPosition.Normalize();
            isPlayerPosition = true;
            Debug.Log("1");
        }
        if (isPlayerPosition)
        {
            //�� ��ǥ�� ����
            bossRB.velocity = playerPosition * attackPlayerSpeed;
            Debug.Log("2");
        }
        if (isTouchingWall || isTouchingDown)
        {
            isPlayerPosition = false;
            //stun animation
            bossRB.velocity = Vector2.zero;
            bossAnimation.SetTrigger("Stuned");
            Debug.Log("3");
        }
    }

    private void FlipTowardPlayer()
    {
        float playerDirection = player.transform.position.x - transform.position.x;
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
        Camera.VibrateFormTime(0.05f);
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
        if (currentHP == 5f)
        {
            bossBGM.ChangeBgm(BGMType.BossHpHalf);
        }
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

    //public void BossHpState()
    //{
          
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

    //private void ChangeBgm()
    //{   
    //    bossBGM.ChangeBgm(BGMType.BossHpHalf);
    //}
    public void BossDie()
    {
        //animator.SetTrigger("IsDead");
        //yield return new WaitForSeconds(1.0f);
        //// ���� ������Ʈ ����
        Destroy(gameObject);
        dieParticle.SetActive(true);
        nextScene.SetActive(true);
    }
}
