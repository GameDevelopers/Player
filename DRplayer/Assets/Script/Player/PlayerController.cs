using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // 체력
    //public int health;
    // 움직이는 속도
    public float moveSpeed;
    // 점프
    public float jumpSpeed;
    public int jumpLeft;
    // 벽점프
    public Vector2 climbJumpForce;
    // 떨어지는 속도
    public float fallSpeed;
    // 스프린트(대쉬) - 속도, 시간, 간격
    public float sprintSpeed;
    public float sprintTime;
    public float sprintInterval;
    // 공격 간격
    //public float attackInterval;

    // 공격(위, 정면, 아래)
    //public Vector2 attackUpRecoil;
    //public Vector2 attackForwardRecoil;
    //public Vector2 attackDownRecoil;

    // 공격이펙트(위, 정면, 아래)
    //public GameObject attackUpEffect;
    //public GameObject attackForwardEffect;
    //public GameObject attackDownEffect;

    // 땅에 닿았는가
    public bool isGround;
    // 벽에 닿았는가
    private bool isClimb;
    // 스프린트 가능한가
    private bool isSprintable;
    // 스프린트 초기화
    private bool isSprintReset;
    // 입력 되었는가
    private bool isInputEnabled;
    // 떨어지고 있는가
    private bool isFalling;
    // 공격가능?
    //private bool isAttackable;

    // 벽점프 딜레이
    private float climbJumpDelay = 0.2f;
    // 공격이펙트 생성시간
    //private float attackEffectLifeTime = 0.05f;

    //private Animator animator;
    private Rigidbody2D playerRigidbody;
    private Transform playerTransform;
    //private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        isInputEnabled = true;
        isSprintReset = true;
        //isAttackable = true;

        //animator = gameObject.GetComponent<Animator>();
        playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
        playerTransform = gameObject.GetComponent<Transform>();
        //spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        updatePlayerState();
        if (isInputEnabled)
        {
            move();
            jumpControl();
            fallControl();
            sprintControl();
            //attackControl();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 벽점프 돌입!
        // 만약 태그가 Wall이고 땅에 닿지 않았다면,
        if (collision.collider.tag == "Wall" && !isGround)
        {
            playerRigidbody.gravityScale = 0;

            Vector2 newVelocity;
            newVelocity.x = 0;
            newVelocity.y = -2;

            playerRigidbody.velocity = newVelocity;

            isClimb = true;
            //animator.SetBool("IsClimb", true);

            isSprintable = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // 만약 태그가 벽이고, 떨어지고 있으며, 벽에 닿지 않았다면
        if (collision.collider.tag == "Wall" && isFalling && !isClimb)
        {
            OnCollisionEnter2D(collision);
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        // 벽점프 끝
        if (collision.collider.tag == "Wall")
        {
            isClimb = false;
            //animator.SetBool("IsClimb", false);

            playerRigidbody.gravityScale = 1;
        }
    }

    /* ######################################################### */

    // 플레이어 상태 메서드
    public void updatePlayerState()
    {
        isGround = checkGrounded();
        //animator.SetBool("IsGround", isGround);

        float verticalVelocity = playerRigidbody.velocity.y;
        //animator.SetBool("IsDown", verticalVelocity < 0);

        if (isGround && verticalVelocity == 0)
        {
            //animator.SetBool("IsJump", false);
            //animator.ResetTrigger("IsJumpFirst");
            //animator.ResetTrigger("IsJumpSecond");
            //animator.SetBool("IsDown", false);

            jumpLeft = 2;
            isClimb = false;
            isSprintable = true;
        }
        else if (isClimb)
        {
            // 벽점프 후 점프는 1번
            jumpLeft = 1;            
        }
    }

    // 플레이어 움직임 메서드
    private void move()
    {
        // 양쪽 움직임은 속도에 비례하게
        float horizonMove = Input.GetAxis("Horizontal") * moveSpeed;

        // set velocity
        Vector2 newVelocity;
        newVelocity.x = horizonMove;
        newVelocity.y = playerRigidbody.velocity.y;
        playerRigidbody.velocity = newVelocity;

        if (!isClimb)
        {
            // the sprite itself is inversed 
            float moveDirection = -transform.localScale.x * horizonMove;

            if (moveDirection < 0)
            {
                // flip player sprite
                Vector3 newScale;
                newScale.x = horizonMove < 0 ? 1 : -1;
                newScale.y = 1;
                newScale.z = 1;

                transform.localScale = newScale;

                //if (isGround)
                //{
                //    // turn back animation
                //    animator.SetTrigger("IsRotate");
                //}
            }
            //else if (moveDirection > 0)
            //{
            //    // move forward
            //    animator.SetBool("IsRun", true);
            //}
        }

        // stop
        //if (Input.GetAxis("Horizontal") == 0)
        //{
        //    animator.SetTrigger("stopTrigger");
        //    animator.ResetTrigger("IsRotate");
        //    animator.SetBool("IsRun", false);
        //}
        //else
        //{
        //    animator.ResetTrigger("stopTrigger");
        //}
    }

    // 점프 (space)
    private void jumpControl()
    {
        if (!Input.GetKeyDown(KeyCode.C)) //(!Input.GetButtonDown("Jump"))
            return;

        if (isClimb)
        {
            climbJump();
        }
        else if (jumpLeft > 0)
            jump();
    }

    // 떨어짐 구현
    private void fallControl()
    {
        if (Input.GetKeyUp(KeyCode.C) && !isClimb) //(Input.GetButtonUp("Jump") && !isClimb)
        {
            isFalling = true;
            fall();
        }
        else
        {
            isFalling = false;
        }
    }

    // 대쉬(x)
    private void sprintControl()
    {
        if (Input.GetKeyDown(KeyCode.X) && isSprintable && isSprintReset)
            sprint();
    }

    // 공격(z)
    //private void attackControl()
    //{
    //    if (Input.GetKeyDown(KeyCode.Z) && !isClimb && isAttackable)
    //        attack();
    //}

    /* ######################################################### */

    // 바닥 체크
    private bool checkGrounded()
    {
        Vector2 origin = transform.position;

        float radius = 0.2f;

        // 아래쪽 감지
        Vector2 direction;
        direction.x = 0;
        direction.y = -1;

        float distance = 1f;
        
        // 레이어 = Platform
        LayerMask layerMask = LayerMask.GetMask("Platform");
        // 레이어가 Platform인 친구의 충돌 정보를 저장(원점, 반경, 방향, 거리, 레이어).
        RaycastHit2D hitPlatform = Physics2D.CircleCast(origin, radius, direction, distance, layerMask);
        // 
        return hitPlatform.collider != null;
    }

    // 점프 메서드
    private void jump()
    {
        Vector2 newVelocity;
        newVelocity.x = playerRigidbody.velocity.x;
        newVelocity.y = jumpSpeed;

        playerRigidbody.velocity = newVelocity;

        //animator.SetBool("IsJump", true);
        jumpLeft -= 1;
        Debug.Log("점프");
        //if (jumpLeft == 0)
        //{
        //    animator.SetTrigger("IsJumpSecond");
        //}
        //else if (jumpLeft == 1)
        //{
        //    animator.SetTrigger("IsJumpFirst");
        //}
    }

    // 벽 점프 메서드
    private void climbJump()
    {
        Vector2 realClimbJumpForce;
        realClimbJumpForce.x = climbJumpForce.x * transform.localScale.x;
        realClimbJumpForce.y = climbJumpForce.y;
        playerRigidbody.AddForce(realClimbJumpForce, ForceMode2D.Impulse);

        //animator.SetTrigger("IsClimbJump");
        //animator.SetTrigger("IsJumpFirst");
        Debug.Log("벽점");

        isInputEnabled = false;

        StartCoroutine(climbJumpCoroutine(climbJumpDelay));
    }

    private IEnumerator climbJumpCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        isInputEnabled = true;

        //animator.ResetTrigger("IsClimbJump");

        // jump to the opposite direction
        Vector3 newScale;
        newScale.x = -transform.localScale.x;
        newScale.y = 1;
        newScale.z = 1;

        transform.localScale = newScale;
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
        newVelocity.x = transform.localScale.x * (isClimb ? sprintSpeed : -sprintSpeed);
        newVelocity.y = 0;

        playerRigidbody.velocity = newVelocity;

        if (isClimb)
        {
            // sprint to the opposite direction
            Vector3 newScale;
            newScale.x = -transform.localScale.x;
            newScale.y = 1;
            newScale.z = 1;

            transform.localScale = newScale;
        }

        //animator.SetTrigger("IsSprint");
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
    //private void attack()
    //{
    //    float verticalDirection = Input.GetAxis("Vertical");
    //    if (verticalDirection > 0)
    //        attackUp();
    //    else if (verticalDirection < 0 && !isGround)
    //        attackDown();
    //    else
    //        attackForward();
    //}

    // 공격(위) 메서드
    //private void attackUp()
    //{
    //    //animator.SetTrigger("IsAttackUp");
    //    //attackUpEffect.SetActive(true);

    //    Vector2 detectDirection;
    //    detectDirection.x = 0;
    //    detectDirection.y = 1;

    //    //StartCoroutine(attackCoroutine(attackUpEffect, attackEffectLifeTime, attackInterval, detectDirection, attackUpRecoil));
    //}

    // 공격(정면) 메서드
    //private void attackForward()
    //{
    //    //animator.SetTrigger("IsAttack");
    //    //attackForwardEffect.SetActive(true);

    //    Vector2 detectDirection;
    //    detectDirection.x = -transform.localScale.x;
    //    detectDirection.y = 0;

    //    Vector2 recoil;
    //    recoil.x = transform.localScale.x > 0 ? -attackForwardRecoil.x : attackForwardRecoil.x;
    //    recoil.y = attackForwardRecoil.y;

        //StartCoroutine(attackCoroutine(attackForwardEffect, attackEffectLifeTime, attackInterval, detectDirection, recoil));
    //}

    // 공격() 메서드
    //private void attackDown()
    //{
    //    //animator.SetTrigger("IsAttackDown");
    //    //attackDownEffect.SetActive(true);

    //    Vector2 detectDirection;
    //    detectDirection.x = 0;
    //    detectDirection.y = -1;

    //    //StartCoroutine(attackCoroutine(attackDownEffect, attackEffectLifeTime, attackInterval, detectDirection, attackDownRecoil));
    //}

    // 공격 코루틴.. 적이 좀 필요할듯.
    //private IEnumerator attackCoroutine(GameObject attackEffect, float effectDelay, float attackInterval, Vector2 detectDirection, Vector2 attackRecoil)
    //{
    //    Vector2 origin = transform.position;

    //    float radius = 0.6f;

    //    float distance = 1.5f;
    //    LayerMask layerMask = LayerMask.GetMask("Enemy") | LayerMask.GetMask("Trap") | LayerMask.GetMask("Switch") | LayerMask.GetMask("Projectile");

    //    RaycastHit2D[] hitRecList = Physics2D.CircleCastAll(origin, radius, detectDirection, distance, layerMask);

    //    foreach (RaycastHit2D hitRec in hitRecList)
    //    {
    //        GameObject obj = hitRec.collider.gameObject;

    //        string layerName = LayerMask.LayerToName(obj.layer);

    //        if (layerName == "Switch")
    //        {
    //            Switch swithComponent = obj.GetComponent<Switch>();
    //            if (swithComponent != null)
    //                swithComponent.turnOn();
    //        }
    //        else if (layerName == "Enemy")
    //        {
    //            EnemyController enemyController = obj.GetComponent<EnemyController>();
    //            if (enemyController != null)
    //                enemyController.hurt(1);
    //        }
    //        else if (layerName == "Projectile")
    //        {
    //            Destroy(obj);
    //        }
    //    }

    //    if (hitRecList.Length > 0)
    //    {
    //        rigidbody.velocity = attackRecoil;
    //    }

    //    yield return new WaitForSeconds(effectDelay);

    //    attackEffect.SetActive(false);

    //    // attack cool down
    //    isAttackable = false;
    //    yield return new WaitForSeconds(attackInterval);
    //    isAttackable = true;
    //}
}
