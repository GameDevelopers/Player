using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �����̴� �ӵ�
    public float moveSpeed;
    // ���� ��.
    public float jumpForce;
    // ���� Ƚ��.
    public int jumpCount;
    // ������
    public Vector2 climbJumpForce;
    // �������� �ӵ�
    public float fallSpeed;
    // ������Ʈ(�뽬) - �ӵ�, �ð�, ����
    public float sprintSpeed;
    public float sprintTime;
    public float sprintInterval;
    // ���� ����
    public float attackInterval;

    // ����(��, ����, �Ʒ�)
    public Vector2 attackUpRecoil;
    public Vector2 attackForwardRecoil;
    public Vector2 attackDownRecoil;

    // ��������Ʈ(��, ����, �Ʒ�)
    public GameObject attackUpEffect;
    public GameObject attackForwardEffect;
    public GameObject attackDownEffect;

    // ���� ��Ҵ°�.
    public bool isGround;
    // ���� ��Ҵ°�.
    private bool isClimb;
    // ������Ʈ ��������.
    private bool isSprintable;
    // ������Ʈ.
    private bool isSprintReset;
    // �Է� �Ǿ��°�.
    private bool isInputEnabled;
    // �������� �ִ°�.
    private bool isFalling;
    // ���ݰ���?
    private bool isAttackable;

    // ������ ������.
    private float climbJumpDelay = 0.2f;
    // ��������Ʈ �����ð�.
    private float attackEffectLifeTime = 0.05f;

    private Animator animator;
    private Rigidbody2D playerRigidbody;
    private Transform playerTransform;
    private AudioSource audioSource;
    public AudioClip attakSound;
    //public AudioClip moveSound;
    public AudioClip jumpSound;
    public AudioClip landingSound;
    public AudioClip sprintSound;

    [SerializeField]
    public GameObject jumpEffectPrefab;
    private bool spawnEffect;



    private void Start()
    {
        // �Է� �ʱ�ȭ.
        isInputEnabled = true;
        // ������Ʈ �ʱ�ȭ.
        isSprintReset = true;
        // ���� �����ϰ� �ʱ�ȭ.
        isAttackable = true;

        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // �Է��ϴ� �Ϳ� ���� �÷��̾��� ���¸� ��ȭ.
        updatePlayerState();
        if (isInputEnabled)
        {
            // ������.
            move();
            // ����.
            jumpControl();
            // �������� �ӵ� ���� ( ���� Ű ���Է� ).
            fallControl();
            // �뽬.
            sprintControl();
            // ����.
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

            // �������� �ӵ��� y���� -2�� �ӵ���.
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
        if (isGround == true)
        {
            if (spawnEffect == true)
            {
                Instantiate(jumpEffectPrefab, playerTransform.position, Quaternion.identity);
                spawnEffect = false;
            }
            else
            {
                spawnEffect = true;
            }
        }
    }

    // �÷��̾� ������ �޼���
    private void move()
    {
        // ���� �������� �ӵ��� ����ϰ�
        float horizonMove = Input.GetAxis("Horizontal") * moveSpeed;
        // �ӵ� ����
        Vector2 newVelocity;
        // x�� ���� ������
        newVelocity.x = horizonMove;
        // y�� �÷��̾��� y��
        newVelocity.y = playerRigidbody.velocity.y;
        // �÷��̾��� �ӵ��� ������ ������ ������ �ʱ�ȭ.
        playerRigidbody.velocity = newVelocity;

        // ���� ��� ���� ������.
        if (!isClimb)
        {
            // �̵����⿡ ���� �÷��̾��� ��������Ʈ�� ����.
            float moveDirection = -playerTransform.localScale.x * horizonMove;
            
            // ���� �÷��̾��� �̵������� 0���� �۴ٸ�.
            if (moveDirection < 0)
            {
                // �÷��̾��� ��������Ʈ�� ����.
                Vector3 newScale;
                // x = �������� 0���� �۴ٸ� ���(����������) �ƴϸ� ����(��������)
                newScale.x = horizonMove < 0 ? 1 : -1;
                // y, z�� ����.
                newScale.y = 1;
                newScale.z = 1;
                
                // �÷��̾��� ũ�⸦ ������ ������ ������ �ʱ�ȭ.
                playerTransform.localScale = newScale;

                // ���࿡ ���� ��Ҵٸ�.
                if (isGround)
                {
                    // �ǵ����� �ִϸ��̼� on.
                    animator.SetTrigger("IsRotate");
                }
            }
            // �� ���࿡ �̵������� 0���� ũ�ٸ�(��
            else if (moveDirection > 0)
            {
                // �޸��� �ִϸ��̼� on.
                animator.SetBool("IsRun", true);
                
            }
        }

        // ���� ���� �����ӿ� ���� �ƹ� �Է��� ���ٸ�.
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

    // ���� (space)
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
    }

    // �������� �ӵ� ��Ʈ�� ����
    private void fallControl()
    {
        // ���� C��ư�� ������ �ְ� ���� ���� �ʾҴٸ�.
        if (Input.GetKeyUp(KeyCode.C) && !isClimb) //(Input.GetButtonUp("Jump") && !isClimb)
        {
            // �������� ���� 
            isFalling = true;
            // ������ �޼��� ����.
            fall();
        }
        // �ƴ϶��
        else
        {
            // �������� ���� ����.
            isFalling = false;
        }
    }

    // �뽬(x)
    private void sprintControl()
    {
        // ���� X��ư�� ������ �ְ� �뽬�� �����ϸ� �뽬�� ��Ÿ���� �ʱ�ȭ �Ǿ��ٸ�.
        if (Input.GetKeyDown(KeyCode.X) && isSprintable && isSprintReset)
        {
            // �뽬 �޼��� ����.
            sprint();
        }
    }

    // ����(z)
    private void attackControl()
    {
        // ���� Z��ư�� ������ �ְ� ���� ���� �ʾ����� ������ ������ ���¶��.
        if (Input.GetKeyDown(KeyCode.Z) && !isClimb && isAttackable)
        {
            // ���� �޼��� ����.
            attack();
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

        //
        // ���� ���� �ִϸ��̼� on.
        animator.SetBool("IsJump", true);
        // ���� �� ����Ƚ�� -1.
        jumpCount -= 1;
        Debug.Log("����");
        PlaySound("JUMP");
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

    // ������ �޼���
    private void climbJump()
    {
        // ������ �� ���� ����.
        Vector2 realClimbJumpForce;
        // x��ǥ�� ������ ��
        realClimbJumpForce.x = climbJumpForce.x * playerTransform.localScale.x;
        realClimbJumpForce.y = climbJumpForce.y;
        playerRigidbody.AddForce(realClimbJumpForce, ForceMode2D.Impulse);

        animator.SetTrigger("IsClimbJump");
        animator.SetTrigger("IsJumpFirst");
        Debug.Log("����");

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

    // �������� �� ���� �޼���
    private void fall()
    {
        Vector2 newVelocity;
        newVelocity.x = playerRigidbody.velocity.x;
        newVelocity.y = -fallSpeed;

        playerRigidbody.velocity = newVelocity;
    }

    // ������Ʈ �޼���
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

    // ���� �޼���
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

    // ����(��) �޼���
    private void attackUp()
    {
        animator.SetTrigger("IsAttackUp");
        attackUpEffect.SetActive(true);

        Vector2 detectDirection;
        detectDirection.x = 0;
        detectDirection.y = -1;

        StartCoroutine(attackCoroutine(attackUpEffect, attackEffectLifeTime, attackInterval, detectDirection, attackUpRecoil));
    }

    // ����(����) �޼���
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

    // ����() �޼���
    private void attackDown()
    {
        animator.SetTrigger("IsAttackDown");
        attackDownEffect.SetActive(true);

        Vector2 detectDirection;
        detectDirection.x = 0;
        detectDirection.y = 1;

        StartCoroutine(attackCoroutine(attackDownEffect, attackEffectLifeTime, attackInterval, detectDirection, attackDownRecoil));
    }

    // ���� �ڷ�ƾ
    private IEnumerator attackCoroutine(GameObject attackEffect, float effectDelay, float attackInterval, Vector2 detectDirection, Vector2 attackRecoil)
    {
        Vector2 origin = playerTransform.position;

        float radius = 0.5f;

        float distance = -3f;
        LayerMask layerMask = LayerMask.GetMask("Enemy") | LayerMask.GetMask("Boss");
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
            //case "MOVE":
            //    audioSource.clip = moveSound;
            //    break;
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


