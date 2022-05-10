using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("MOVE")]
    public float moveSpeed; // �����̴� �ӵ�
    private float horizonMove; // ���� ������ Ű�� ���� ����
    private bool faceRight = true; // �������� ���� �ִ°�

    [Header("JUMP + SPRINT")]  
    public float jumpForce; // ���� ��.    
    public int jumpCount; // ���� Ƚ��.
    public Vector2 climbJumpForce;  // ������    
    public float fallSpeed; // �������� �ӵ�

    // ������Ʈ(�뽬) - �ӵ�, �ð�, ����
    public float sprintSpeed;
    public float sprintTime;
    public float sprintInterval;

    [Header("ATTACK")]
    public float attackInterval; // ���� ����

    // ����(��, ����, �Ʒ�)�ݵ� ����
    public Vector2 attackUpRecoil;
    public Vector2 attackForwardRecoil;
    public Vector2 attackDownRecoil;

    // ��������Ʈ(��, ����, �Ʒ�)
    public GameObject attackUpEffect;
    public GameObject attackForwardEffect;
    public GameObject attackDownEffect;

    [Header("BOOL")]
    public bool isGround;        // ���� ��Ҵ°�.
    private bool isClimb;        // ���� ��Ҵ°�.
    public bool isClimbable;     // ������ �����Ѱ�.
    private bool isSprintable;   // ������Ʈ ��������.
    private bool isSprintReset;  // ������Ʈ ��Ÿ�� ����.
    private bool isInputEnabled; // �Է��� �ִ°�.
    private bool isFalling;      // �������� �ִ°�.
    private bool isAttackable;   // ���ݰ����Ѱ�

    [Header("FLOAT")]
    private float climbJumpDelay = 0.2f; // ������ ������.
    private float attackEffectLifeTime = 0.05f; // ��������Ʈ �����ð�.

    [Header("OTHERS")]
    private Animator animator;
    private Rigidbody2D playerRigidbody;
    private Transform playerTransform;
    // ����� �ҽ� �� Ŭ��
    private AudioSource audioSource;
    public AudioClip attakSound;
    public AudioClip moveSound;
    public AudioClip jumpSound;
    public AudioClip landingSound;
    public AudioClip sprintSound;
   
    // �ʱ�ȭ �� ������Ʈ ������ ���� Start�޼���
    private void Start()
    {
        // �Է� �ʱ�ȭ.
        isInputEnabled = true;
        // ������Ʈ �ʱ�ȭ.
        isSprintReset = true;
        // ���� �����ϰ� �ʱ�ȭ.
        isAttackable = true;

        // ����
        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
    }

    // �÷��̾��� ���� ��ȭ�� ���� Update�޼���
    private void Update()
    {
        // �Է��ϴ� �Ϳ� ���� �÷��̾��� ���¸� ��ȭ.
        updatePlayerState();
        if (isInputEnabled)
        {
            // ������ �޼���.
            move();
            // ���� ���� �޼���.
            jumpControl();
            // �������� �ӵ� ����( ���� Ű ���Է� ).
            fallControl();
            // �뽬 ���� �޼���.
            sprintControl();
            // ���� ���� �޼���.
            attackControl();
        }
    }

    // ���� �� ����� ��.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ������ ����!
        // ���� �±װ� Wall�̰� ���� ���� �ʾҴٸ�,
        if (collision.collider.tag == "Wall" && !isGround)
        {
            // �÷��̾��� �߷��� ũ�⸦ 0.
            playerRigidbody.gravityScale = 0;

            // �������� �ӵ��� ���͸� y���� -2�� �ӵ���.
            Vector2 newVelocity;
            newVelocity.x = 0;
            newVelocity.y = -2;

            // �÷��̾��� �������� �ӵ��� ������ ���� ������ ũ��� �ʱ�ȭ.
            playerRigidbody.velocity = newVelocity;

            // ���� ���� ����.
            isClimb = true;
            // �� ���� �ִϸ����� ����.
            animator.SetBool("IsClimb", true);

            // �뽬���� ����.
            isSprintable = true;
        }
    }

    // ���� ��� �ִ� ���� ��.
    private void OnCollisionStay2D(Collision2D collision)
    {
        // ���� �±װ� ���̰�, �������� ������, ���� ���� �ʾҴٸ�.
        if (collision.collider.tag == "Wall" && isFalling && !isClimb)
        {
            // ���� ���� �� ����� �� �޼��带 ����.
            OnCollisionEnter2D(collision);
        }
    }

    // ���� �Ÿ��� ������ �� ( = ������ �� ������ �־����� ��).
    private void OnCollisionExit2D(Collision2D collision)
    {
        // ������ ��.
        if (collision.collider.tag == "Wall")
        {
            // ���� �ȴ���.
            isClimb = false;
            // �� ���� �ִϸ��̼� off.
            animator.SetBool("IsClimb", false);

            // �÷��̾��� �߷��� �ٽ� 1��.
            playerRigidbody.gravityScale = 1;
        }
    }

    // �÷��̾� ���� �޼���
    public void updatePlayerState()
    {
        // ���� ��Ҵ��� üũ.
        isGround = checkGrounded();
        // ���� ������� �ִϸ������� IsGround üũ.
        animator.SetBool("IsGround", isGround);


        // �������� �ӵ� = �÷��̾��� y�� 
        float verticalVelocity = playerRigidbody.velocity.y;
        // �������� �ӵ��� ���� 0���� �������� ����.
        animator.SetBool("IsDown", verticalVelocity < 0);

        // ���� ����ְ� �������� �ӵ��� 0�̶��.
        if (isGround && verticalVelocity == 0)
        {
            // �ִϸ������� �������� off.
            animator.SetBool("IsJump", false);
            // �ִϸ������� 1�� ���� Ʈ���Ÿ� ����
            animator.ResetTrigger("IsJumpFirst");
            // �ִϸ������� 2�� ���� Ʈ���Ÿ� ����
            animator.ResetTrigger("IsJumpSecond");
            // �������� ���� ����.
            animator.SetBool("IsDown", false);

            // ���� Ƚ���� 2ȸ.
            jumpCount = 2;
            // ���� �ȴ���.
            isClimb = false;
            // �뽬 ����.
            isSprintable = true;
        }
        // ���� ����ִٸ�.
        else if (isClimb)
        {
            // ���� 1�� ����.
            jumpCount = 1;
        }
    }

    // �÷��̾��� �������� ����ϴ� �޼���
    private void move()
    {
        // ���� �������� �ӵ��� ����ϰ�
        horizonMove = Input.GetAxis("Horizontal") * moveSpeed;
        // �ӵ����� ����
        Vector2 newVelocity;
        // x�� ���� ������
        newVelocity.x = horizonMove;
        // y�� ���� �÷��̾ ���� y��
        newVelocity.y = playerRigidbody.velocity.y;
        // �÷��̾��� �ӵ��� ������ ������ ������ �ʱ�ȭ.
        playerRigidbody.velocity = newVelocity;

        // ���� ��� ���� ������.
        if (!isClimbable)
        {
            // �̵����⿡ ���� �÷��̾��� ��������Ʈ�� ����.
            float moveDirection = horizonMove;

            // ���� �÷��̾��� �̵������� 0���� �۴ٸ�.
            if (moveDirection < 0 && !faceRight)
            {
                // ��������Ʈ ����.
                Flip();
                // MOVE ����� ����
                PlaySound("MOVE");
                animator.SetBool("IsRun", true);

                // ���࿡ ���� ��Ҵٸ�.
                if (isGround)
                {
                    // �ǵ����� �ִϸ��̼� on.
                    animator.SetTrigger("IsRotate");
                }
            }

            // �� ���࿡ �̵������� 0���� ũ�ٸ�(��
            else if (moveDirection > 0 && faceRight)
            {
                // ��������Ʈ ����.
                Flip();
                // �޸��� �ִϸ��̼� on.
                animator.SetBool("IsRun", true);
            }
        }

        // ���� �ƹ� �Է��� ���ٸ�.
        if (Input.GetAxis("Horizontal") == 0)
        {
            // �����ִ� �ִϸ��̼� on.
            animator.SetTrigger("stopTrigger");
            // ȸ���� �ִϸ��̼� ����.
            animator.ResetTrigger("IsRotate");
            // �޸��� ���� ���� ����.
            animator.SetBool("IsRun", false);
        }
        // �Է����̶��.
        else
        {
            // ��������� �ִϸ��̼� ����.
            animator.ResetTrigger("stopTrigger");
        }
    }

    // �÷��̾� sprite �ø�
    void Flip()
    {
        // �������� ���� ���� �ʴٸ�
        faceRight = !faceRight;
        // ����
        transform.Rotate(0f, 180f, 0f);
    }

    // �÷��̾� �������¸� �����ϴ� �޼��� (KEY = X)
    private void jumpControl()
    {
        // ���� Ű������ C��ư�� ������ ���� ������ ���� x.
        if (!Input.GetKeyDown(KeyCode.C)) //(!Input.GetButtonDown("Jump"))
            return;

        // ���� ���� ����ִٸ�.
        if (isClimb)
        {
            // ������ �޼��� ����.
            climbJump();
        }
        // ���࿡ ����Ƚ���� 0���� ũ��
        else if (jumpCount > 0)
        {
            // ���� �޼��� ����.
            jump();
        }
        // JUMP����� Ŭ�� ���
        PlaySound("JUMP");
    }

    // ���� �ӵ� ��Ʈ�� �޼���
    private void fallControl()
    {
        // ���� C��ư�� ������ �ְ� ���� ���� �ʾҴٸ�.
        if (Input.GetKeyUp(KeyCode.C) && !isClimb)
        {
            // �������� ���� 
            isFalling = true;
            // ���� �޼��� ����.
            fall();
        }
        // �ƴ϶��
        else
        {
            // �������� ���� ����.
            isFalling = false;
        }
    }

    // �÷��̾� �뽬���¸� �����ϴ� �޼��� (KEY = X)
    private void sprintControl()
    {
        // ���� X��ư�� ������ �ְ� �뽬�� �����ϸ� �뽬�� ��Ÿ���� �ʱ�ȭ �Ǿ��ٸ�.
        if (Input.GetKeyDown(KeyCode.X) && isSprintable && isSprintReset)
        {
            // �뽬 �޼��� ����.
            sprint();
            // SPRINT����� Ŭ�� ���
            PlaySound("SPRINT");
        }
    }

    // �÷��̾� ���ݻ��¸� �����ϴ� �޼��� ( KEY = Z )
    private void attackControl()
    {
        // ���� Z��ư�� ������ �ְ� ���� ���� �ʾ����� ������ ������ ���¶��.
        if (Input.GetKeyDown(KeyCode.Z) && !isClimb && isAttackable)
        {
            // ���� �޼��� ����.
            attack();
            // ATTACK����� Ŭ�� ���
            PlaySound("ATTACK");
        }

    }


    // �ٴ� üũ �޼���
    private bool checkGrounded()
    {
        // ���� ��ǥ�� �÷��̾��� ��ġ.
        Vector2 origin = playerTransform.position;

        // �ݰ��� 0.2f.
        float radius = 0.2f;

        // ���� ���� ���� ����.
        Vector2 direction;
        // ���� ����(0, -1).
        direction.x = 0;
        direction.y = -1;

        // �Ÿ��� 1.
        float distance = 1f;
        // �Ʒ��� ����.
        // ������ ���̾� = Platform
        LayerMask layerMask = LayerMask.GetMask("Platform");
        // ������ �ʴ� ������ ������ ���� ���̸� �߻��Ͽ� 
        // ���̾ Platform�� ģ���� �浹 ������ ����
        // (����, �ݰ�, ����, �Ÿ�, ���̾�)
        RaycastHit2D hitPlatform = Physics2D.CircleCast(origin, radius, direction, distance, layerMask);
        return hitPlatform.collider != null;
    }

    // ���� �޼���.
    private void jump()
    {
        // ���ο� �ӵ� ���� ����.
        Vector2 newVelocity;
        // x���� �÷��̾��� x�ӵ� ��.
        newVelocity.x = playerRigidbody.velocity.x;
        // y���� ������.
        newVelocity.y = jumpForce;

        // �÷��̾��� �ӵ��� �޼��� �ʹݿ� ���� ���� ������ �ʱ�ȭ.
        playerRigidbody.velocity = newVelocity;

        // ���� ���� �ִϸ��̼� on.
        animator.SetBool("IsJump", true);
        // ���� �� ����Ƚ�� -1.
        jumpCount -= 1;
        // ���� ���� ���� Ƚ���� 0�̶��.
        if (jumpCount == 0)
        {
            // 2������ �ִϸ��̼� on.
            animator.SetTrigger("IsJumpSecond");
        }
        // ���� ���� Ƚ���� 1�̶��.
        else if (jumpCount == 1)
        {
            // 1������ �ִϸ��̼� on.
            animator.SetTrigger("IsJumpFirst");
        }
       
    }

    //������ �޼���
    private void climbJump()
    {
        // ������ �� ���� ����.
        Vector2 realClimbJumpForce;
        // x�� ������ �� * �÷��̾��� x���� * 0.5
        realClimbJumpForce.x = climbJumpForce.x * playerTransform.localScale.x * 0.5f;
        realClimbJumpForce.y = climbJumpForce.y;
        // �÷��̾��� Rigidbody�� ������ ���� ���͸�ŭ�� ���� �߰�
        // �Ű������� ��/���� ������ �ӵ��� ����
        playerRigidbody.AddForce(realClimbJumpForce, ForceMode2D.Impulse);

        // �ִϸ��̼� on
        animator.SetTrigger("IsClimbJump");
        animator.SetTrigger("IsJumpFirst");

        // �Է� x
        isInputEnabled = false;

        // ������ �ڷ�ƾ ����
        StartCoroutine(climbJumpCoroutine(climbJumpDelay));
    }

    // �÷��̾� �������� ���� �ڷ�ƾ
    private IEnumerator climbJumpCoroutine(float delay)
    {
        // ��Ÿ�� : �� �� ����ǰ� �� �� �� ����
        yield return new WaitForSeconds(delay);

        // �Է� o
        isInputEnabled = true;
        
        // �ִϸ��̼� on
        animator.ResetTrigger("IsClimbJump");

        // �ݴ� �������� ����
        Vector3 newScale;
        newScale.x = -playerTransform.localScale.x;
        newScale.y = 1;
        newScale.z = 1;

        playerTransform.localScale = newScale;
    }

    // �÷��̾� ���� �ӵ� ���� �޼���
    private void fall()
    {
        // �� ���� ����
        Vector2 newVelocity;
        // x : �÷��̾� ���� x��, y : -������ŭ�� ��
        newVelocity.x = playerRigidbody.velocity.x;
        newVelocity.y = -fallSpeed;

        // �÷��̾��� ���� ���� �� ���Ͱ����� ����
        playerRigidbody.velocity = newVelocity;
    }

    // �뽬 �޼���
    private void sprint()
    {
        // �Է��� x
        isInputEnabled = false;
        // �뽬 �Ұ�
        isSprintable = false;
        // �뽬 ��Ÿ�� x 
        isSprintReset = false;

        // �� ���� ����
        Vector2 newVelocity;
        // x : ���� �������� Input�� * �뽬 �ӵ�
        newVelocity.x = -horizonMove * 0.5f * (isClimb ? sprintSpeed : -sprintSpeed);
        // y : ��ȭ x
        newVelocity.y = 0;

        // �÷��̾� �ӵ����� ���� ������ ����
        playerRigidbody.velocity = newVelocity;

        // ��Ÿ�� ���̶��
        if (isClimb)
        {
            // �ݴ� �������� �뽬 ����
            Vector3 newScale;
            newScale.x = playerTransform.localScale.x;
            newScale.y = 1;
            newScale.z = 1;

            playerTransform.localScale = newScale;
        }
        // IsSprit�Ķ���� �ߵ�
        animator.SetTrigger("IsSprint");
        // �뽬 �ڷ�ƾ(�뽬 �ð� �� �Ÿ�) ����
        StartCoroutine(sprintCoroutine(sprintTime, sprintInterval));
    }

    // �뽬 �ڷ�ƾ
    private IEnumerator sprintCoroutine(float sprintDelay, float sprintInterval)
    {
        // �����ð� ��- �뽬 ��Ÿ���� ������
        yield return new WaitForSeconds(sprintDelay);
        // �Է� ����
        isInputEnabled = true;
        // �뽬 ����
        isSprintable = true;

        // ���� �ð� ��- �뽬 �Ÿ����� ���ٸ�
        yield return new WaitForSeconds(sprintInterval);
        // �뽬 ��Ÿ��
        isSprintReset = true;
    }

    // ���� �޼���
    private void attack()
    {
        // ���� ������ Input���� ��� ���� ����
        float verticalDirection = Input.GetAxis("Vertical");
        // ���� 0���� ũ�ٸ� (�� ����Ű)
        if (verticalDirection > 0)
            // �� ����
            attackUp();
        // 0���� �۰�(�Ʒ� ����Ű) ���� ���� �ʾҴٸ�
        else if (verticalDirection < 0 && !isGround)
            // �Ʒ� ����
            attackDown();
        // �� ��
        else
            // ���� ����
            attackForward();
    }

    // ����(��) �޼���
    private void attackUp()
    {
        // IsAttackUp�Ķ���� �ߵ�
        animator.SetTrigger("IsAttackUp");
        // �÷��̾� ������ �־�� UpAttackImage������Ʈ �ѱ�
        attackUpEffect.SetActive(true);

        // �� ���� ���� ����(0, 1)
        Vector2 detectDirection;
        detectDirection.x = 0;
        detectDirection.y = 1;

        // ���� �ڷ�ƾ ����(���� ���ϴ� ����Ʈ, ����Ʈ ���� �ð�, ���� �Ÿ�, ���� ����, �ݵ�)
        StartCoroutine(attackCoroutine(attackUpEffect, attackEffectLifeTime, attackInterval, detectDirection, attackUpRecoil));
    }

    // ����(����) �޼���
    private void attackForward()
    {
        // IsAttack�Ķ���� �ߵ�
        animator.SetTrigger("IsAttack");
        // �÷��̾� ������ �־�� AttackImage������Ʈ �ѱ�
        attackForwardEffect.SetActive(true);

        // �� ���� ���� ����(���� �������� Input��, 0)
        Vector2 detectDirection;
        detectDirection.x = horizonMove;
        detectDirection.y = 0;

        // ���� ���� �� ����� �ݵ� ���� ���� ���� ����
        Vector2 recoil;
        // x : ���� �������� Input���� 0���� ū�� �������� ���� �ݴ� �������� �ݵ�
        recoil.x = horizonMove > 0 ? -attackForwardRecoil.x : attackForwardRecoil.x;
        // y : ������ ��(0)
        recoil.y = attackForwardRecoil.y;

        // ���� �ڷ�ƾ ����(���� ����Ʈ, ����Ʈ ���� �ð�, ���� �Ÿ�, ���� ����, �ݵ�)
        StartCoroutine(attackCoroutine(attackForwardEffect, attackEffectLifeTime, attackInterval, detectDirection, recoil));
    }

    // ����() �޼���
    private void attackDown()
    {
        // IsAttackDown�Ķ���� �ߵ�
        animator.SetTrigger("IsAttackDown");
        // �÷��̾� ������ �־�� DownAttackImage������Ʈ �ѱ�
        attackDownEffect.SetActive(true);

        // �� ���� ���� ����(0, -1)
        Vector2 detectDirection;
        detectDirection.x = 0;
        detectDirection.y = -1;

        // ���� �ڷ�ƾ ����(�Ʒ��� ���ϴ� ����Ʈ, ����Ʈ ���� �ð�, ���� �Ÿ�, ���� ����, �ݵ�)
        StartCoroutine(attackCoroutine(attackDownEffect, attackEffectLifeTime, attackInterval, detectDirection, attackDownRecoil));
    }

    // ���� �ڷ�ƾ(����Ʈ, ����Ʈ �ð�, ���� �Ÿ�, ���� ����, �ݵ�)
    private IEnumerator attackCoroutine(GameObject attackEffect, float effectDelay, float attackInterval, Vector2 detectDirection, Vector2 attackRecoil)
    {
        // �÷��̾��� ���� ��ġ ���� ���ο� ���ͷ� ����
        Vector2 origin = playerTransform.position;

        // ����ĳ��Ʈ �ν� �ݰ�
        float radius = 0.5f;

        // ����ĳ���� �ν� �Ÿ�
        float distance = 0.8f;
        // �ν��� ���̾�(��, ����, ��)
        LayerMask layerMask = LayerMask.GetMask("Enemy") | LayerMask.GetMask("Boss") | LayerMask.GetMask("Door"); ;
        // ���̰� �� ������ �� Ȯ�ο�
        Debug.DrawRay(origin, detectDirection, Color.red, 1f);
        // ����ĳ��Ʈ�� �迭�� ����� Ư�� ���̾� Ȯ��( ��ǥ, �ݰ�, ����, �Ÿ�, ���̾�)
        RaycastHit2D[] hitRecList = Physics2D.CircleCastAll(origin, radius, detectDirection, distance, layerMask);

        // ���̰� �ν��� ���� �� �迭�ȿ� �ִٸ�
        foreach (RaycastHit2D hitRec in hitRecList)
        {
            // �ν��� ������Ʈ�� �ݶ��̴� ����
            GameObject obj = hitRec.collider.gameObject;
           
            string layerName = LayerMask.LayerToName(obj.layer);

            // ���� ���̾� �̸��� Enemy���
            if (layerName == "Enemy")
            {
                // Enemy ������Ʈ�� ������Ʈ�� ������ 
                Enemy enemy = obj.GetComponent<Enemy>();
                // ���� ���� �����Ѵٸ�
                if (enemy != null)
                    // Enemy��ũ��Ʈ �� �� �ǰ� �޼��� ����
                    enemy.HitDamage(1);

            }
            // ���� ���̾� �̸��� Boss���
            else if (layerName == "Boss")
            {
                // Boss ������Ʈ�� ������Ʈ�� ������ 
                Boss boss = obj.GetComponent<Boss>();
                // ���� ���� �����Ѵٸ�
                if (boss != null)
                    // Boss��ũ��Ʈ �� ���� �ǰ� �޼��� ����
                    boss.BossDamaged(1);

            }
            // ���� ���̾� �̸��� Door���
            else if (layerName == "Door")
            {
                // door ������Ʈ�� ������Ʈ�� ������ 
                DoorCrush door = obj.GetComponent<DoorCrush>();
                // DoorCrush��ũ��Ʈ �� �� �ǰ� �޼��� ����
                door.HitDoor(1);
            }

        }

        // ���� �� �迭�� index���� 0���� ũ�ٸ�
        if (hitRecList.Length > 0)
        {
            // �÷��̾��� ���͸� �ݵ� ������ ����.
            playerRigidbody.velocity = attackRecoil;
        }

        // ����Ʈ�� �� ���� ��
        yield return new WaitForSeconds(effectDelay);

        // ����Ʈ ������Ʈ ����
        attackEffect.SetActive(false);

        // ���� ��Ÿ��
        // ���ݺҰ�
        isAttackable = false;
        // ������ �� ���� ��
        yield return new WaitForSeconds(attackInterval);
        // ���� ����
        isAttackable = true;
    }

    // �÷��̾�� ���õ� ����� ���带 �����ϴ� �޼���
    private void PlaySound(string action)
    {
        // "action"
        switch (action)
        {
            // ���� "action"�� "JUMP"���
            case "JUMP":
                audioSource.clip = jumpSound; // ���� ����� Ŭ�� ���
                break;
            // ���� "action"�� "ATTACK"���
            case "ATTACK":
                audioSource.clip = attakSound; // ���� ����� Ŭ�� ���
                break;
            // ���� "action"�� "MOVE"���
            case "MOVE":
                audioSource.clip = moveSound; // ������ ����� Ŭ�� ���
                break;
            // ���� "action"�� "LANDING"���
            case "LANDING":
                audioSource.clip = landingSound; // ���� ����� Ŭ�� ���
                break;
            // ���� "action"�� "SPRINT"���
            case "SPRINT":
                audioSource.clip = sprintSound; // �뽬 ����� Ŭ�� ���
                break;          
        }
        audioSource.Play();
    }
}


