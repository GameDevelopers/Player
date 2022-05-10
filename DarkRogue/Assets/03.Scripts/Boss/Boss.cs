using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    // 보스 체력
    [Header("Boss HP")]
    [SerializeField]
    private float maxHP = 10;    // 보스 최대 HP
    private float currentHP;    // 현재 보스의 HP
    public int bossDamage = 1;    // 보스 공격력
    private SpriteRenderer spriteRenderer;
    public float MaxHP => maxHP;    // HpView 스크립트에서 쓰기 위함.
    public float CurrentHP => currentHP;    // HpView 스크립트에서 쓰기 위함.

    [Header("BossMove")]
    [SerializeField] private float bossMoveSpeed; // 보스의 움직임 속도
    [SerializeField] private Vector2 bossMoveDirection; // 보스 움직임 방향

    [Header("AttackUp&Down")]
    [SerializeField] private float attackMoveSpeed; // 공격 시 움직이는 속도
    [SerializeField] private Vector2 attackMoveDirection; // 공격 시 움직이는 방향

    [Header("AttackPlayer")]
    [SerializeField] private float attackPlayerSpeed; // 공격 시 플레이어에게 향하는 속도 
    [SerializeField] private GameObject player; // 플레이어의 위치값을 가져오기 위한 오브젝트
    [SerializeField]
    private Vector3 playerPosition; // 플레이어 위치
    [SerializeField]
    private bool isPlayerPosition; // 플레이어 위치가 찍혔는가?

    [Header("Others")]
    [SerializeField] private Transform platformCheckUp; // 맵 위 체크
    [SerializeField] private Transform platformCheckDown; // 맵 아래 체크
    [SerializeField] private Transform platformCheckWall; // 벽 체크
    [SerializeField] private float platformCheckRadius; // 레이캐스트 시 플랫폼을 체크하기 위한 반경
    [SerializeField] private LayerMask platformLayer; // 플랫폼 레이어
    private bool isTouchingUp; // 위에 닿았는가
    private bool isTouchingDown; // 아래 닿았는가
    private bool isTouchingWall; // 벽에 닿았는가
    private bool goingUp = true; // 올라가는 상태 bool
    private bool facingLeft = true; // 왼쪽보는 상태 bool
    private Rigidbody2D bossRB;
    private Animator bossAnimation;
    // 배경음악 설정(보스피깎 시 변경)
    [SerializeField]
    private BossMapBGM bossBGM;
    [SerializeField]
    private GameObject nextScene;
    [SerializeField]
    private GameObject dieParticle;
    // 보스가 맵 내 콜라이더에 닿았을 때 카메라 흔들림 설정.
    [SerializeField]
    private CameraShake Camera;

    // 참조 불러오기 및 초기화
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

    // 보스가 매 순간 위, 아래, 벽 체크
    private void Update()
    {
        // 해당 레이어 마스크로 지정된 게임 오브젝트의 존재 여부를 검사하여, 발견시 참(True) 값을 반환
        // 좌표, 반지름, 레이어
        isTouchingUp = Physics2D.OverlapCircle(platformCheckUp.position, platformCheckRadius, platformLayer);
        isTouchingDown = Physics2D.OverlapCircle(platformCheckDown.position, platformCheckRadius, platformLayer);
        isTouchingWall = Physics2D.OverlapCircle(platformCheckWall.position, platformCheckRadius, platformLayer);
        AttackPlayer();
    }

    // 상태머신 내에서 해당 에니매이션 실행을 랜덤으로 주기 위함 = 알 수 없는 패턴 
    private void randomStatePick()
    {
        // 0과 1 중 한 개의 값
        int randomState = Random.Range(0, 2);
        // 만약 0이면,
        if (randomState == 0)
        {
            // AttackUPDown 애니메이션
            bossAnimation.SetTrigger("AttackUpDown");
        }
        // 1이면,
        else if (randomState == 1)
        {
            // AttackPlayer 애니메이션
            bossAnimation.SetTrigger("AttackPlayer");
        }
    }

    // 보스 상태 메서드
    public void BossState()
    {
        // 만약 위쪽에 닿았고 올라가는 중이라면
        if (isTouchingUp && goingUp)
        {
            // 이 메서드 실행
            ChangeDirection();
        }
        // 아래 닿았고 올라가지 않는다면
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }
        // 벽에 닿았다면
        if (isTouchingWall)
        {
            // 왼쪽을 보고 있다면
            if (facingLeft)
            {
                // 스프라이트 반전
                Flip();
            }
            // 오른쪽을 보고 있다면
            else if (!facingLeft)
            {
                Flip();
            }
        }
        // 보스의 속도 = 속도 * 방향
        bossRB.velocity = bossMoveSpeed * bossMoveDirection;
    }

    // 위 아래 공격 메서드
    public void AttackUpNDown()
    {
        // 만약 위에 닿았고 올라가는 중이라면
        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
        }
        // 아래 닿았고 올라가는 중이 아니라면
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }
        // 벽에 닿았고 왼쪽을 보면
        if (isTouchingWall && facingLeft)
        {
            Flip();
        }
        // 벽에 닿았고 오른쪽을 보고 있으면
        else if (!facingLeft && isTouchingWall)
        {
            Flip();
        }
        // 보스 속도 = 공격 시 속도 * 공격 시 방향
        bossRB.velocity = attackMoveSpeed * attackMoveDirection;
    }

    // 플레이어 향하는 공격 메서드
    public void AttackPlayer()
    {
        // 만약 플레이어 위치값이 없다면
        if (!isPlayerPosition)
        {
            // 플레이어 좌표
            playerPosition = player.transform.position + transform.position;
            // 플레이어를 향해 스프라이트를 반전하는 메서드
            FlipTowardPlayer();
            //플레이어 좌표 노멀라이즈
            playerPosition.Normalize();
            // 플레이어 위치값 초기화
            isPlayerPosition = true;
        }
        // 위치값이 존재한다면
        if (isPlayerPosition)
        {
            //위 좌표로 공격(플레이어 위치 벡터 * 플레이어를 향한 속도)
            bossRB.velocity = playerPosition * attackPlayerSpeed;
        }
        // 벽에 닿았거나 아래 닿으면
        if (isTouchingWall || isTouchingDown)
        {
            // 플레이어 위치값 초기화
            isPlayerPosition = false;
            // 보스의 속도를 0으로 만든 후 스턴 애니메이션
            bossRB.velocity = Vector2.zero;
            bossAnimation.SetTrigger("Stuned");
        }
    }

    // 플레이어가 보스를 지나치면 따라갈 수 있게 스프라이트를 반전해주는 메서드
    private void FlipTowardPlayer()
    {
        // 플레이어 방향
        float playerDirection = player.transform.position.x - transform.position.x;
        // 플레이어 방향이 양수(우측)이고 보스가 왼쪽을 바라보고 있다면
        if (playerDirection > 0 && facingLeft)
        {
            // 스프라이트 반전.
            Flip();
        }
        // 플레이어 방향이 음수(좌측)이고 보스가 오른쪽을 바라보고 있다면
        else if (playerDirection < 0 && !facingLeft)
        {
            Flip();
        }
    }

    // 방향을 전환해주는 메서드
    private void ChangeDirection()
    {
        // 올라가는 상태를 올라가지 않는 상태로 변경
        goingUp = !goingUp;
        // 보스의 y움직임을 반대로
        bossMoveDirection.y *= -1;
        // 공격시 보스의 움직임을 반대로
        attackMoveDirection.y *= -1;
        // 메인 카메라 진동 시간
        Camera.VibrateFormTime(0.05f);
    }

    // 보스의 스프라이트를 반전시키는 메서드
    private void Flip()
    {
        facingLeft = !facingLeft;
        // 보스가 움직이는 방향을 반대로
        bossMoveDirection.x *= -1;
        // 공격 시 보스의 움직임방향을 반대로
        attackMoveDirection.x *= -1;
        // 오브젝트의 transform의 Rotate값을 180으로 주어 스프라이트 반전
        transform.Rotate(0, 180, 0);
    }

    // 디버깅 - 인스펙터 뷰에서 보스가 현재 위, 아래, 벽에 닿았는지 상황 체크
    private void OnDrawGizmosSelected()
    {
        // 색상 변경
        Gizmos.color = Color.cyan;
        // 구체로 
        Gizmos.DrawWireSphere(platformCheckUp.position, platformCheckRadius);
        Gizmos.DrawWireSphere(platformCheckDown.position, platformCheckRadius);
        Gizmos.DrawWireSphere(platformCheckWall.position, platformCheckRadius);
    }

    // 보스가 입는 데미지
    public void BossDamaged(float damage)
    {
        // 만약 현재 체력이 2f라면
        if (currentHP == 2f)
        {
            // bossScene의 음악을 변경
            bossBGM.ChangeBgm(BGMType.BossHpHalf);
        }
        // 만약 현재 hp가 0보다 작거나 같다면
        if (currentHP == 0)
        {
            // 체력이 0이면 보스 사망.
            BossDie();
        }
        // 현재 HP를 -damage만큼
        currentHP -= damage;
        // 아래 작성해둔 색상 변경 코루틴 정지 및 시작
        StopCoroutine("HitAnimation");
        StartCoroutine("HitAnimation");
    }


    // 플레이어와 충돌하면
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // 적 공격력만큼 플레이어 체력감소
            collision.GetComponent<Health>().TakeDamage(bossDamage);
        }
    }

    // 보스 피격 시 색상 변경 코루틴
    private IEnumerator HitAnimation()
    {
        // 색상 변경
        spriteRenderer.color = Color.black;
        //0.1초 대기
        yield return new WaitForSeconds(0.1f);
        // 원래 색상으로
        spriteRenderer.color = Color.white;
    }

    public void BossDie()
    {
        // 보스 오브젝트 삭제
        Destroy(gameObject);
        // die 파티클 오브젝트 on
        dieParticle.SetActive(true);
        // 다음씬으로 가는 스크립트를 넣어둔 오브젝트 on
        nextScene.SetActive(true);
    }
}
