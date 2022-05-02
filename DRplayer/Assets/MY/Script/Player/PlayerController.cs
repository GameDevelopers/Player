using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 움직이는 속도
    public float moveSpeed;
    // 점프 힘.
    public float jumpForce;
    // 점프 횟수.
    public int jumpCount;
    // 벽점프
    public Vector2 climbJumpForce;
    // 떨어지는 속도
    public float fallSpeed;
    // 스프린트(대쉬) - 속도, 시간, 간격
    public float sprintSpeed;
    public float sprintTime;
    public float sprintInterval;
    // 공격 간격
    public float attackInterval;

    // 공격(위, 정면, 아래)
    public Vector2 attackUpRecoil;
    public Vector2 attackForwardRecoil;
    public Vector2 attackDownRecoil;

    // 공격이펙트(위, 정면, 아래)
    public GameObject attackUpEffect;
    public GameObject attackForwardEffect;
    public GameObject attackDownEffect;

    // 땅에 닿았는가.
    public bool isGround;
    // 벽에 닿았는가.
    private bool isClimb;
    // 스프린트 가능한지.
    private bool isSprintable;
    // 스프린트.
    private bool isSprintReset;
    // 입력 되었는가.
    private bool isInputEnabled;
    // 떨어지고 있는가.
    private bool isFalling;
    // 공격가능?
    private bool isAttackable;

    // 벽점프 딜레이.
    private float climbJumpDelay = 0.2f;
    // 공격이펙트 생성시간.
    private float attackEffectLifeTime = 0.05f;

    private Animator animator;
    private Rigidbody2D playerRigidbody;
    private Transform playerTransform;
    private AudioSource audioSource;
    public AudioClip attakSound;
    public AudioClip moveSound;
    public AudioClip jumpSound;
    public AudioClip landingSound;
    public AudioClip sprintSound;



    private void Start()
    {
        // 입력 초기화.
        isInputEnabled = true;
        // 스프린트 초기화.
        isSprintReset = true;
        // 공격 가능하게 초기화.
        isAttackable = true;

        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // 입력하는 것에 따라 플레이어의 상태를 변화.
        updatePlayerState();
        if (isInputEnabled)
        {
            // 움직임.
            move();
            // 점프.
            jumpControl();
            // 떨어지는 속도 조절 ( 점프 키 재입력 ).
            fallControl();
            // 대쉬.
            sprintControl();
            // 공격.
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

            // 떨어지는 속도를 y기준 -2의 속도로.
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

    // 플레이어 움직임 메서드
    private void move()
    {
        // 양쪽 움직임은 속도에 비례하게
        float horizonMove = Input.GetAxis("Horizontal") * moveSpeed;
        // 속도 설정
        Vector2 newVelocity;
        // x는 양쪽 움직임
        newVelocity.x = horizonMove;
        // y는 플레이어의 y값
        newVelocity.y = playerRigidbody.velocity.y;
        // 플레이어의 속도를 위에서 지정한 값으로 초기화.
        playerRigidbody.velocity = newVelocity;
      
        // 벽에 닿아 있지 않으면.
        if (!isClimb)
        {
            // 이동방향에 따라 플레이어의 스프라이트가 반전.
            float moveDirection = -playerTransform.localScale.x * horizonMove;
            
            // 만약 플레이어의 이동방향이 0보다 작다면.
            if (moveDirection < 0)
            {
                // 플레이어의 스프라이트가 반전.
                Vector3 newScale;
                // x = 움직임이 0보다 작다면 양수(오른쪽으로) 아니면 음수(왼쪽으로)
                newScale.x = horizonMove < 0 ? 1 : -1;
                // y, z값 노상관.
                newScale.y = 1;
                newScale.z = 1;
                PlaySound("MOVE");

                // 플레이어의 크기를 위에서 설정한 것으로 초기화.
                playerTransform.localScale = newScale;

                // 만약에 땅에 닿았다면.
                if (isGround)
                {
                    // 되돌리는 애니매이션 on.
                    animator.SetTrigger("IsRotate");
                }
            }
            // 또 만약에 이동방향이 0보다 크다면(오
            else if (moveDirection > 0)
            {
                // 달리는 애니메이션 on.
                animator.SetBool("IsRun", true);
            }
        }

        // 만약 양쪽 움직임에 대해 아무 입력이 없다면.
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

    // 점프 (X)
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
        PlaySound("JUMP");
    }

    // 떨어지는 속도 컨트롤 구현
    private void fallControl()
    {
        // 만약 C버튼을 누르고 있고 벽에 닿지 않았다면.
        if (Input.GetKeyUp(KeyCode.C) && !isClimb) //(Input.GetButtonUp("Jump") && !isClimb)
        {
            // 떨어지는 상태 
            isFalling = true;
            // 떨어짐 메서드 실행.
            fall();
        }
        // 아니라면
        else
        {
            // 떨어지고 있지 않음.
            isFalling = false;
        }
    }

    // 대쉬(x)
    private void sprintControl()
    {
        // 만약 X버튼을 누르고 있고 대쉬가 가능하며 대쉬의 쿨타임이 초기화 되었다면.
        if (Input.GetKeyDown(KeyCode.X) && isSprintable && isSprintReset)
        {
            // 대쉬 메서드 실행.
            sprint();
            PlaySound("SPRINT");
        }
    }

    // 공격(z)
    private void attackControl()
    {
        // 만약 Z버튼을 누르고 있고 벽에 닿지 않았으며 공격이 가능한 상태라면.
        if (Input.GetKeyDown(KeyCode.Z) && !isClimb && isAttackable)
        {
            // 공격 메서드 실행.
            attack();
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

        //
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

    // 벽점프 메서드
    private void climbJump()
    {
        // 벽점프 힘 벡터 생성.
        Vector2 realClimbJumpForce;
        // x좌표는 벽점프 힘
        realClimbJumpForce.x = climbJumpForce.x * playerTransform.localScale.x;
        realClimbJumpForce.y = climbJumpForce.y;
        playerRigidbody.AddForce(realClimbJumpForce, ForceMode2D.Impulse);

        animator.SetTrigger("IsClimbJump");
        animator.SetTrigger("IsJumpFirst");
        Debug.Log("벽점");

        isInputEnabled = false;

        StartCoroutine(climbJumpCoroutine(climbJumpDelay));
    }

    private IEnumerator climbJumpCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        isInputEnabled = true;

        animator.ResetTrigger("IsClimbJump");

        // jump to the opposite direction
        Vector3 newScale;
        newScale.x = -playerTransform.localScale.x;
        newScale.y = 1;
        newScale.z = 1;

        playerTransform.localScale = newScale;
    }

    // 떨어지는 거 구현 메서드
    private void fall()
    {
        Vector2 newVelocity;
        newVelocity.x = playerRigidbody.velocity.x;
        newVelocity.y = -fallSpeed;

        playerRigidbody.velocity = newVelocity;
    }

    // 스프린트 메서드
    private void sprint()
    {
        // reject input during sprinting
        isInputEnabled = false;
        isSprintable = false;
        isSprintReset = false;

        Vector2 newVelocity;
        newVelocity.x = playerTransform.localScale.x * (isClimb ? sprintSpeed : -sprintSpeed);
        newVelocity.y = 0;

        playerRigidbody.velocity = newVelocity;

        if (isClimb)
        {
            // sprint to the opposite direction
            Vector3 newScale;
            newScale.x = -playerTransform.localScale.x;
            newScale.y = 1;
            newScale.z = 1;

            playerTransform.localScale = newScale;
        }

        animator.SetTrigger("IsSprint");
        StartCoroutine(sprintCoroutine(sprintTime, sprintInterval));
    }

    private IEnumerator sprintCoroutine(float sprintDelay, float sprintInterval)
    {
        yield return new WaitForSeconds(sprintDelay);
        isInputEnabled = true;
        isSprintable = true;

        yield return new WaitForSeconds(sprintInterval);
        isSprintReset = true;
    }

    // 공격 메서드
    private void attack()
    {
        float verticalDirection = Input.GetAxis("Vertical");
        if (verticalDirection > 0)
            attackUp();
        else if (verticalDirection < 0 && !isGround)
            attackDown();
        else
            attackForward();
    }

    // 공격(위) 메서드
    private void attackUp()
    {
        animator.SetTrigger("IsAttackUp");
        attackUpEffect.SetActive(true);

        Vector2 detectDirection;
        detectDirection.x = 0;
        detectDirection.y = -1;

        StartCoroutine(attackCoroutine(attackUpEffect, attackEffectLifeTime, attackInterval, detectDirection, attackUpRecoil));
    }

    // 공격(정면) 메서드
    private void attackForward()
    {
        animator.SetTrigger("IsAttack");
        attackForwardEffect.SetActive(true);

        Vector2 detectDirection;
        detectDirection.x = playerTransform.localScale.x;
        detectDirection.y = 0;

        Vector2 recoil;
        recoil.x = playerTransform.localScale.x > 0 ? -attackForwardRecoil.x : attackForwardRecoil.x;
        recoil.y = attackForwardRecoil.y;

        StartCoroutine(attackCoroutine(attackForwardEffect, attackEffectLifeTime, attackInterval, detectDirection, recoil));
    }

    // 공격() 메서드
    private void attackDown()
    {
        animator.SetTrigger("IsAttackDown");
        attackDownEffect.SetActive(true);

        Vector2 detectDirection;
        detectDirection.x = 0;
        detectDirection.y = 1;

        StartCoroutine(attackCoroutine(attackDownEffect, attackEffectLifeTime, attackInterval, detectDirection, attackDownRecoil));
    }

    // 공격 코루틴
    private IEnumerator attackCoroutine(GameObject attackEffect, float effectDelay, float attackInterval, Vector2 detectDirection, Vector2 attackRecoil)
    {
        Vector2 origin = playerTransform.position;

        float radius = 0.5f;

        float distance = -3f;
        LayerMask layerMask = LayerMask.GetMask("Enemy") | LayerMask.GetMask("Boss") | LayerMask.GetMask("Door");
        Debug.DrawRay(origin, detectDirection, Color.red, 1f);
        RaycastHit2D[] hitRecList = Physics2D.CircleCastAll(origin, radius, detectDirection, distance, layerMask);

        foreach (RaycastHit2D hitRec in hitRecList)
        {
            GameObject obj = hitRec.collider.gameObject;

            string layerName = LayerMask.LayerToName(obj.layer);

            if (layerName == "Enemy")
            {
                Enemy enemy = obj.GetComponent<Enemy>();
                if (enemy != null)
                    enemy.HitDamage(1);

            }
            else if (layerName == "Boss")
            {
                Boss boss = obj.GetComponent<Boss>();
                if (boss != null)
                    boss.BossDamaged(1);

            }
            else if (layerName == "Door")
            {
                DoorCrush door = obj.GetComponent<DoorCrush>();
                door.HitDoor(1);
            }

        }

        if (hitRecList.Length > 0)
        {
            playerRigidbody.velocity = attackRecoil;
        }

        yield return new WaitForSeconds(effectDelay);

        attackEffect.SetActive(false);

        // attack cool down
        isAttackable = false;
        yield return new WaitForSeconds(attackInterval);
        isAttackable = true;
    }

    private void PlaySound(string action)
    {
        switch (action)
        {
            case "JUMP":
                audioSource.clip = jumpSound;
                break;
            case "ATTACK":
                audioSource.clip = attakSound;
                break;
            case "MOVE":
                audioSource.clip = moveSound;
                break;
            case "LANDING":
                audioSource.clip = landingSound;
                break;
            case "SPRINT":
                audioSource.clip = sprintSound;
                break;
            //case "DIE":
            //    audioSource.clip = dieSound;
            //    break;
        }
        audioSource.Play();
    }
}


