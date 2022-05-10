using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("MOVE")]
    public float moveSpeed; // 움직이는 속도
    private float horizonMove; // 양쪽 움직임 키를 담을 변수
    private bool faceRight = true; // 오른쪽을 보고 있는가

    [Header("JUMP + SPRINT")]  
    public float jumpForce; // 점프 힘.    
    public int jumpCount; // 점프 횟수.
    public Vector2 climbJumpForce;  // 벽점프    
    public float fallSpeed; // 떨어지는 속도

    // 스프린트(대쉬) - 속도, 시간, 간격
    public float sprintSpeed;
    public float sprintTime;
    public float sprintInterval;

    [Header("ATTACK")]
    public float attackInterval; // 공격 간격

    // 공격(위, 정면, 아래)반동 벡터
    public Vector2 attackUpRecoil;
    public Vector2 attackForwardRecoil;
    public Vector2 attackDownRecoil;

    // 공격이펙트(위, 정면, 아래)
    public GameObject attackUpEffect;
    public GameObject attackForwardEffect;
    public GameObject attackDownEffect;

    [Header("BOOL")]
    public bool isGround;        // 땅에 닿았는가.
    private bool isClimb;        // 벽에 닿았는가.
    public bool isClimbable;     // 벽점프 가능한가.
    private bool isSprintable;   // 스프린트 가능한지.
    private bool isSprintReset;  // 스프린트 쿨타임 리셋.
    private bool isInputEnabled; // 입력이 있는가.
    private bool isFalling;      // 떨어지고 있는가.
    private bool isAttackable;   // 공격가능한가

    [Header("FLOAT")]
    private float climbJumpDelay = 0.2f; // 벽점프 딜레이.
    private float attackEffectLifeTime = 0.05f; // 공격이펙트 생성시간.

    [Header("OTHERS")]
    private Animator animator;
    private Rigidbody2D playerRigidbody;
    private Transform playerTransform;
    // 오디오 소스 및 클립
    private AudioSource audioSource;
    public AudioClip attakSound;
    public AudioClip moveSound;
    public AudioClip jumpSound;
    public AudioClip landingSound;
    public AudioClip sprintSound;
   
    // 초기화 및 컴포넌트 참조를 위한 Start메서드
    private void Start()
    {
        // 입력 초기화.
        isInputEnabled = true;
        // 스프린트 초기화.
        isSprintReset = true;
        // 공격 가능하게 초기화.
        isAttackable = true;

        // 참조
        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
    }

    // 플레이어의 상태 변화를 위한 Update메서드
    private void Update()
    {
        // 입력하는 것에 따라 플레이어의 상태를 변화.
        updatePlayerState();
        if (isInputEnabled)
        {
            // 움직임 메서드.
            move();
            // 점프 관리 메서드.
            jumpControl();
            // 떨어지는 속도 조절( 점프 키 재입력 ).
            fallControl();
            // 대쉬 관리 메서드.
            sprintControl();
            // 공격 관리 메서드.
            attackControl();
        }
    }

    // 벽에 막 닿았을 때.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 벽점프 시작!
        // 만약 태그가 Wall이고 땅에 닿지 않았다면,
        if (collision.collider.tag == "Wall" && !isGround)
        {
            // 플레이어의 중력의 크기를 0.
            playerRigidbody.gravityScale = 0;

            // 떨어지는 속도의 벡터를 y기준 -2의 속도로.
            Vector2 newVelocity;
            newVelocity.x = 0;
            newVelocity.y = -2;

            // 플레이어의 떨어지는 속도를 위에서 만든 벡터의 크기로 초기화.
            playerRigidbody.velocity = newVelocity;

            // 벽에 닿은 상태.
            isClimb = true;
            // 벽 점프 애니메이터 실행.
            animator.SetBool("IsClimb", true);

            // 대쉬가능 상태.
            isSprintable = true;
        }
    }

    // 벽에 닿고 있는 중일 때.
    private void OnCollisionStay2D(Collision2D collision)
    {
        // 만약 태그가 벽이고, 떨어지고 있으며, 벽에 닿지 않았다면.
        if (collision.collider.tag == "Wall" && isFalling && !isClimb)
        {
            // 위의 벽에 막 닿았을 때 메서드를 실행.
            OnCollisionEnter2D(collision);
        }
    }

    // 벽과 거리가 생겼을 때 ( = 벽점프 후 벽에서 멀어졌을 때).
    private void OnCollisionExit2D(Collision2D collision)
    {
        // 벽점프 끝.
        if (collision.collider.tag == "Wall")
        {
            // 벽에 안닿음.
            isClimb = false;
            // 벽 점프 애니메이션 off.
            animator.SetBool("IsClimb", false);

            // 플레이어의 중력을 다시 1로.
            playerRigidbody.gravityScale = 1;
        }
    }

    // 플레이어 상태 메서드
    public void updatePlayerState()
    {
        // 땅에 닿았는지 체크.
        isGround = checkGrounded();
        // 땅에 닿았으면 애니메이터의 IsGround 체크.
        animator.SetBool("IsGround", isGround);


        // 떨어지는 속도 = 플레이어의 y값 
        float verticalVelocity = playerRigidbody.velocity.y;
        // 떨어지는 속도의 값이 0보다 떨어지는 상태.
        animator.SetBool("IsDown", verticalVelocity < 0);

        // 땅에 닿아있고 떨어지는 속도가 0이라면.
        if (isGround && verticalVelocity == 0)
        {
            // 애니메이터의 점프상태 off.
            animator.SetBool("IsJump", false);
            // 애니메이터의 1단 점프 트리거를 종료
            animator.ResetTrigger("IsJumpFirst");
            // 애니메이터의 2단 점프 트리거를 종료
            animator.ResetTrigger("IsJumpSecond");
            // 떨어지는 상태 종료.
            animator.SetBool("IsDown", false);

            // 점프 횟수는 2회.
            jumpCount = 2;
            // 벽에 안닿음.
            isClimb = false;
            // 대쉬 가능.
            isSprintable = true;
        }
        // 벽에 닿아있다면.
        else if (isClimb)
        {
            // 점프 1번 가능.
            jumpCount = 1;
        }
    }

    // 플레이어의 움직임을 담당하는 메서드
    private void move()
    {
        // 양쪽 움직임은 속도에 비례하게
        horizonMove = Input.GetAxis("Horizontal") * moveSpeed;
        // 속도벡터 설정
        Vector2 newVelocity;
        // x는 양쪽 움직임
        newVelocity.x = horizonMove;
        // y는 현재 플레이어가 가진 y값
        newVelocity.y = playerRigidbody.velocity.y;
        // 플레이어의 속도를 위에서 지정한 값으로 초기화.
        playerRigidbody.velocity = newVelocity;

        // 벽에 닿아 있지 않으면.
        if (!isClimbable)
        {
            // 이동방향에 따라 플레이어의 스프라이트가 반전.
            float moveDirection = horizonMove;

            // 만약 플레이어의 이동방향이 0보다 작다면.
            if (moveDirection < 0 && !faceRight)
            {
                // 스프라이트 반전.
                Flip();
                // MOVE 오디오 실행
                PlaySound("MOVE");
                animator.SetBool("IsRun", true);

                // 만약에 땅에 닿았다면.
                if (isGround)
                {
                    // 되돌리는 애니매이션 on.
                    animator.SetTrigger("IsRotate");
                }
            }

            // 또 만약에 이동방향이 0보다 크다면(오
            else if (moveDirection > 0 && faceRight)
            {
                // 스프라이트 반전.
                Flip();
                // 달리는 애니메이션 on.
                animator.SetBool("IsRun", true);
            }
        }

        // 만약 아무 입력이 없다면.
        if (Input.GetAxis("Horizontal") == 0)
        {
            // 멈춰있는 애니메이션 on.
            animator.SetTrigger("stopTrigger");
            // 회전한 애니메이션 종료.
            animator.ResetTrigger("IsRotate");
            // 달리고 있지 않은 상태.
            animator.SetBool("IsRun", false);
        }
        // 입력중이라면.
        else
        {
            // 멈춤상태의 애니메이션 종료.
            animator.ResetTrigger("stopTrigger");
        }
    }

    // 플레이어 sprite 플립
    void Flip()
    {
        // 오른쪽을 보고 있지 않다면
        faceRight = !faceRight;
        // 반전
        transform.Rotate(0f, 180f, 0f);
    }

    // 플레이어 점프상태를 관리하는 메서드 (KEY = X)
    private void jumpControl()
    {
        // 만약 키보드의 C버튼을 누르고 있지 않으면 실행 x.
        if (!Input.GetKeyDown(KeyCode.C)) //(!Input.GetButtonDown("Jump"))
            return;

        // 만약 벽에 닿아있다면.
        if (isClimb)
        {
            // 벽점프 메서드 실행.
            climbJump();
        }
        // 만약에 점프횟수가 0보다 크면
        else if (jumpCount > 0)
        {
            // 점프 메서드 실행.
            jump();
        }
        // JUMP오디오 클립 재생
        PlaySound("JUMP");
    }

    // 낙하 속도 컨트롤 메서드
    private void fallControl()
    {
        // 만약 C버튼을 누르고 있고 벽에 닿지 않았다면.
        if (Input.GetKeyUp(KeyCode.C) && !isClimb)
        {
            // 떨어지는 상태 
            isFalling = true;
            // 낙하 메서드 실행.
            fall();
        }
        // 아니라면
        else
        {
            // 떨어지고 있지 않음.
            isFalling = false;
        }
    }

    // 플레이어 대쉬상태를 관리하는 메서드 (KEY = X)
    private void sprintControl()
    {
        // 만약 X버튼을 누르고 있고 대쉬가 가능하며 대쉬의 쿨타임이 초기화 되었다면.
        if (Input.GetKeyDown(KeyCode.X) && isSprintable && isSprintReset)
        {
            // 대쉬 메서드 실행.
            sprint();
            // SPRINT오디오 클립 재생
            PlaySound("SPRINT");
        }
    }

    // 플레이어 공격상태를 관리하는 메서드 ( KEY = Z )
    private void attackControl()
    {
        // 만약 Z버튼을 누르고 있고 벽에 닿지 않았으며 공격이 가능한 상태라면.
        if (Input.GetKeyDown(KeyCode.Z) && !isClimb && isAttackable)
        {
            // 공격 메서드 실행.
            attack();
            // ATTACK오디오 클립 재생
            PlaySound("ATTACK");
        }

    }


    // 바닥 체크 메서드
    private bool checkGrounded()
    {
        // 원점 좌표는 플레이어의 위치.
        Vector2 origin = playerTransform.position;

        // 반경은 0.2f.
        float radius = 0.2f;

        // 방향 벡터 변수 생성.
        Vector2 direction;
        // 방향 벡터(0, -1).
        direction.x = 0;
        direction.y = -1;

        // 거리는 1.
        float distance = 1f;
        // 아래쪽 감지.
        // 감지할 레이어 = Platform
        LayerMask layerMask = LayerMask.GetMask("Platform");
        // 보이지 않는 원형의 지름을 가진 레이를 발사하여 
        // 레이어가 Platform인 친구의 충돌 정보를 저장
        // (원점, 반경, 방향, 거리, 레이어)
        RaycastHit2D hitPlatform = Physics2D.CircleCast(origin, radius, direction, distance, layerMask);
        return hitPlatform.collider != null;
    }

    // 점프 메서드.
    private void jump()
    {
        // 새로운 속도 벡터 생성.
        Vector2 newVelocity;
        // x값은 플레이어의 x속도 값.
        newVelocity.x = playerRigidbody.velocity.x;
        // y값은 점프힘.
        newVelocity.y = jumpForce;

        // 플레이어의 속도를 메서드 초반에 만든 벡터 값으로 초기화.
        playerRigidbody.velocity = newVelocity;

        // 점프 상태 애니메이션 on.
        animator.SetBool("IsJump", true);
        // 점프 시 점프횟수 -1.
        jumpCount -= 1;
        // 만약 남은 점프 횟수가 0이라면.
        if (jumpCount == 0)
        {
            // 2단점프 애니메이션 on.
            animator.SetTrigger("IsJumpSecond");
        }
        // 남은 점프 횟수가 1이라면.
        else if (jumpCount == 1)
        {
            // 1단점프 애니메이션 on.
            animator.SetTrigger("IsJumpFirst");
        }
       
    }

    //벽점프 메서드
    private void climbJump()
    {
        // 벽점프 힘 벡터 생성.
        Vector2 realClimbJumpForce;
        // x는 벽점프 힘 * 플레이어의 x배율 * 0.5
        realClimbJumpForce.x = climbJumpForce.x * playerTransform.localScale.x * 0.5f;
        realClimbJumpForce.y = climbJumpForce.y;
        // 플레이어의 Rigidbody에 위에서 만든 벡터만큼의 힘을 추가
        // 매개변수를 힘/질량 값으로 속도를 변경
        playerRigidbody.AddForce(realClimbJumpForce, ForceMode2D.Impulse);

        // 애니메이션 on
        animator.SetTrigger("IsClimbJump");
        animator.SetTrigger("IsJumpFirst");

        // 입력 x
        isInputEnabled = false;

        // 벽점프 코루틴 실행
        StartCoroutine(climbJumpCoroutine(climbJumpDelay));
    }

    // 플레이어 벽점프를 위한 코루틴
    private IEnumerator climbJumpCoroutine(float delay)
    {
        // 쿨타임 : 한 번 실행되고 몇 초 후 실행
        yield return new WaitForSeconds(delay);

        // 입력 o
        isInputEnabled = true;
        
        // 애니메이션 on
        animator.ResetTrigger("IsClimbJump");

        // 반대 방향으로 점프
        Vector3 newScale;
        newScale.x = -playerTransform.localScale.x;
        newScale.y = 1;
        newScale.z = 1;

        playerTransform.localScale = newScale;
    }

    // 플레이어 낙하 속도 구현 메서드
    private void fall()
    {
        // 새 벡터 생성
        Vector2 newVelocity;
        // x : 플레이어 현재 x값, y : -변수만큼의 값
        newVelocity.x = playerRigidbody.velocity.x;
        newVelocity.y = -fallSpeed;

        // 플레이어의 현재 값을 새 벡터값으로 변경
        playerRigidbody.velocity = newVelocity;
    }

    // 대쉬 메서드
    private void sprint()
    {
        // 입력중 x
        isInputEnabled = false;
        // 대쉬 불가
        isSprintable = false;
        // 대쉬 쿨타임 x 
        isSprintReset = false;

        // 새 벡터 생성
        Vector2 newVelocity;
        // x : 양쪽 움직임의 Input값 * 대쉬 속도
        newVelocity.x = -horizonMove * 0.5f * (isClimb ? sprintSpeed : -sprintSpeed);
        // y : 변화 x
        newVelocity.y = 0;

        // 플레이어 속도값을 생성 값으로 변경
        playerRigidbody.velocity = newVelocity;

        // 벽타는 중이라면
        if (isClimb)
        {
            // 반대 방향으로 대쉬 가능
            Vector3 newScale;
            newScale.x = playerTransform.localScale.x;
            newScale.y = 1;
            newScale.z = 1;

            playerTransform.localScale = newScale;
        }
        // IsSprit파라미터 발동
        animator.SetTrigger("IsSprint");
        // 대쉬 코루틴(대쉬 시간 및 거리) 실행
        StartCoroutine(sprintCoroutine(sprintTime, sprintInterval));
    }

    // 대쉬 코루틴
    private IEnumerator sprintCoroutine(float sprintDelay, float sprintInterval)
    {
        // 일정시간 후- 대쉬 쿨타임이 지나면
        yield return new WaitForSeconds(sprintDelay);
        // 입력 가능
        isInputEnabled = true;
        // 대쉬 가능
        isSprintable = true;

        // 일정 시간 후- 대쉬 거리까지 갔다면
        yield return new WaitForSeconds(sprintInterval);
        // 대쉬 쿨타임
        isSprintReset = true;
    }

    // 공격 메서드
    private void attack()
    {
        // 수직 방향의 Input값을 담기 위한 변수
        float verticalDirection = Input.GetAxis("Vertical");
        // 만약 0보다 크다면 (위 방향키)
        if (verticalDirection > 0)
            // 위 공격
            attackUp();
        // 0보다 작고(아래 방향키) 땅에 닿지 않았다면
        else if (verticalDirection < 0 && !isGround)
            // 아래 공격
            attackDown();
        // 그 외
        else
            // 정면 공격
            attackForward();
    }

    // 공격(위) 메서드
    private void attackUp()
    {
        // IsAttackUp파라미터 발동
        animator.SetTrigger("IsAttackUp");
        // 플레이어 하위에 넣어둔 UpAttackImage오브젝트 켜기
        attackUpEffect.SetActive(true);

        // 새 방향 벡터 생성(0, 1)
        Vector2 detectDirection;
        detectDirection.x = 0;
        detectDirection.y = 1;

        // 공격 코루틴 실행(위로 향하는 이펙트, 이펙트 생성 시간, 공격 거리, 공격 방향, 반동)
        StartCoroutine(attackCoroutine(attackUpEffect, attackEffectLifeTime, attackInterval, detectDirection, attackUpRecoil));
    }

    // 공격(정면) 메서드
    private void attackForward()
    {
        // IsAttack파라미터 발동
        animator.SetTrigger("IsAttack");
        // 플레이어 하위에 넣어둔 AttackImage오브젝트 켜기
        attackForwardEffect.SetActive(true);

        // 새 방향 벡터 생성(양쪽 움직임의 Input값, 0)
        Vector2 detectDirection;
        detectDirection.x = horizonMove;
        detectDirection.y = 0;

        // 정면 공격 후 생기는 반동 값을 위한 벡터 생성
        Vector2 recoil;
        // x : 양쪽 움직임의 Input값이 0보다 큰지 작은지에 따라 반대 방향으로 반동
        recoil.x = horizonMove > 0 ? -attackForwardRecoil.x : attackForwardRecoil.x;
        // y : 설정한 값(0)
        recoil.y = attackForwardRecoil.y;

        // 공격 코루틴 실행(정면 이펙트, 이펙트 생성 시간, 공격 거리, 공격 방향, 반동)
        StartCoroutine(attackCoroutine(attackForwardEffect, attackEffectLifeTime, attackInterval, detectDirection, recoil));
    }

    // 공격() 메서드
    private void attackDown()
    {
        // IsAttackDown파라미터 발동
        animator.SetTrigger("IsAttackDown");
        // 플레이어 하위에 넣어둔 DownAttackImage오브젝트 켜기
        attackDownEffect.SetActive(true);

        // 새 방향 벡터 생성(0, -1)
        Vector2 detectDirection;
        detectDirection.x = 0;
        detectDirection.y = -1;

        // 공격 코루틴 실행(아래로 향하는 이펙트, 이펙트 생성 시간, 공격 거리, 공격 방향, 반동)
        StartCoroutine(attackCoroutine(attackDownEffect, attackEffectLifeTime, attackInterval, detectDirection, attackDownRecoil));
    }

    // 공격 코루틴(이펙트, 이펙트 시간, 공격 거리, 공격 방향, 반동)
    private IEnumerator attackCoroutine(GameObject attackEffect, float effectDelay, float attackInterval, Vector2 detectDirection, Vector2 attackRecoil)
    {
        // 플레이어의 현재 위치 값을 새로운 벡터로 선언
        Vector2 origin = playerTransform.position;

        // 레이캐스트 인식 반경
        float radius = 0.5f;

        // 레이캐스터 인식 거리
        float distance = 0.8f;
        // 인식할 레이어(적, 보스, 문)
        LayerMask layerMask = LayerMask.GetMask("Enemy") | LayerMask.GetMask("Boss") | LayerMask.GetMask("Door"); ;
        // 레이가 잘 나오는 지 확인용
        Debug.DrawRay(origin, detectDirection, Color.red, 1f);
        // 레이캐스트를 배열로 만들어 특정 레이어 확인( 좌표, 반경, 방향, 거리, 레이어)
        RaycastHit2D[] hitRecList = Physics2D.CircleCastAll(origin, radius, detectDirection, distance, layerMask);

        // 레이가 인식할 값이 위 배열안에 있다면
        foreach (RaycastHit2D hitRec in hitRecList)
        {
            // 인식할 오브젝트의 콜라이더 정보
            GameObject obj = hitRec.collider.gameObject;
           
            string layerName = LayerMask.LayerToName(obj.layer);

            // 만약 레이어 이름이 Enemy라면
            if (layerName == "Enemy")
            {
                // Enemy 오브젝트의 컴포넌트를 가져옴 
                Enemy enemy = obj.GetComponent<Enemy>();
                // 만약 값이 존재한다면
                if (enemy != null)
                    // Enemy스크립트 내 적 피격 메서드 실행
                    enemy.HitDamage(1);

            }
            // 만약 레이어 이름이 Boss라면
            else if (layerName == "Boss")
            {
                // Boss 오브젝트의 컴포넌트를 가져옴 
                Boss boss = obj.GetComponent<Boss>();
                // 만약 값이 존재한다면
                if (boss != null)
                    // Boss스크립트 내 보스 피격 메서드 실행
                    boss.BossDamaged(1);

            }
            // 만약 레이어 이름이 Door라면
            else if (layerName == "Door")
            {
                // door 오브젝트의 컴포넌트를 가져옴 
                DoorCrush door = obj.GetComponent<DoorCrush>();
                // DoorCrush스크립트 내 문 피격 메서드 실행
                door.HitDoor(1);
            }

        }

        // 만약 위 배열의 index값이 0보다 크다면
        if (hitRecList.Length > 0)
        {
            // 플레이어의 벡터를 반동 값으로 변경.
            playerRigidbody.velocity = attackRecoil;
        }

        // 이펙트가 다 나온 후
        yield return new WaitForSeconds(effectDelay);

        // 이펙트 오브젝트 끄기
        attackEffect.SetActive(false);

        // 공격 쿨타임
        // 공격불가
        isAttackable = false;
        // 공격이 다 나간 후
        yield return new WaitForSeconds(attackInterval);
        // 공격 가능
        isAttackable = true;
    }

    // 플레이어와 관련된 오디오 사운드를 관리하는 메서드
    private void PlaySound(string action)
    {
        // "action"
        switch (action)
        {
            // 만약 "action"이 "JUMP"라면
            case "JUMP":
                audioSource.clip = jumpSound; // 점프 오디오 클립 재생
                break;
            // 만약 "action"이 "ATTACK"라면
            case "ATTACK":
                audioSource.clip = attakSound; // 공격 오디오 클립 재생
                break;
            // 만약 "action"이 "MOVE"라면
            case "MOVE":
                audioSource.clip = moveSound; // 움직임 오디오 클립 재생
                break;
            // 만약 "action"이 "LANDING"라면
            case "LANDING":
                audioSource.clip = landingSound; // 착지 오디오 클립 재생
                break;
            // 만약 "action"이 "SPRINT"라면
            case "SPRINT":
                audioSource.clip = sprintSound; // 대쉬 오디오 클립 재생
                break;          
        }
        audioSource.Play();
    }
}


