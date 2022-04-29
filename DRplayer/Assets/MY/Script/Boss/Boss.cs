using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [Header("Boss HP")]
    [SerializeField]
    private float maxHP = 10;    // 보스 최대 HP
    private float currentHP;    // 현재 보스의 HP
    public int bossDamage = 1;    // 보스 공격력
    private SpriteRenderer spriteRenderer;
    public float MaxHP => maxHP;    // HpView 스크립트에서 쓰기 위함.
    public float CurrentHP => currentHP;    // HpView 스크립트에서 쓰기 위함.
                                            
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
    // 배경음악 설정(보스피깎 시 변경)
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
        // 현재 HP를 최대 HP로
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
            // AttackUPDown 애니메이션
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
            //플레이어 좌표
            playerPosition = player.transform.position + transform.position;
            FlipTowardPlayer();
            //플레이어 좌표 노멀라이즈
            playerPosition.Normalize();
            isPlayerPosition = true;
            Debug.Log("1");
        }
        if (isPlayerPosition)
        {
            //위 좌표로 공격
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
    // 보스가 입는 데미지
    public void BossDamaged(float damage)
    {
        if (currentHP == 5f)
        {
            bossBGM.ChangeBgm(BGMType.BossHpHalf);
        }
        //if (isBossDie) return;
        //Debug.Log(isBossDie);
        // 만약 현재 hp가 0보다 작거나 같다면
        if (currentHP == 0)
        {
            // 체력이 0이면 보스 사망.
            BossDie();
            //isBossDie = true;
        }
        // 현재 HP를 -damage만큼
        currentHP -= damage;

        StopCoroutine("HitAnimation");
        StartCoroutine("HitAnimation");
    }

    //public void BossHpState()
    //{
          
    //}

    // 플레이어와 충돌하면
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // 적 공격력만큼 플레이어 체력감소
            collision.GetComponent<Health>().TakeDamage(bossDamage);
        }
    }

    private IEnumerator HitAnimation()
    {
        // 색상 변경
        spriteRenderer.color = Color.black;
        //0.1초 대기
        yield return new WaitForSeconds(0.1f);
        // 원래 색상으로
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
        //// 보스 오브젝트 삭제
        Destroy(gameObject);
        dieParticle.SetActive(true);
        nextScene.SetActive(true);
    }
}
