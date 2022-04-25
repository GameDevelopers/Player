using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BossState { Appear = 0, Phase1, Phase2}

public class Boss : MonoBehaviour
{
    // 보스가 죽었는가
    public bool isBossDie = false;
    // 보스가 나타나는 시간
    [SerializeField]
    private float bossAppear = 2.5f;
    // 보스 상태 - 나타나기
    private BossState bossState = BossState.Appear;
    // 움직임(등장을 위함)
    private Movement movement;
    private BossWeapon bossWeapon;
    // 보스 공격력
    public int bossDamage = 1;
    // 보스 최대 HP
    [SerializeField]
    private float maxHP = 10;
    // 현재 보스의 HP
    private float currentHP;
    //private SpriteRenderer spriteRenderer;

    // HpView 스크립트에서 쓰기 위함.
    public float MaxHP => maxHP;
    // HpView 스크립트에서 쓰기 위함.
    public float CurrentHP => currentHP;
    private Animator animator;
    private Animation anim;


    //[SerializeField]
    //public GameObject BossClearText;

    private void Awake()
    {
        // 참조
        movement = GetComponent<Movement>();
        anim = GetComponent<Animation>();
        animator = GetComponent<Animator>();
        bossWeapon = GetComponent<BossWeapon>();
        // 현재 HP를 최대 HP로
        currentHP = maxHP;
    }

    //public void Update()
    //{
    //    StartCoroutine(Move());
    //}

    // 보스가 입는 데미지
    public void BossDamaged(float damage)
    {
        // 현재 HP를 -damage만큼
        currentHP -= damage;

        //StopCoroutine("HitAnimation");
        //StartCoroutine("HitAnimation");

        // 만약 현재 hp가 0보다 작거나 같다면
        if (currentHP <= 0)
        {
            // 체력이 0이면 보스 사망.
            StartCoroutine("BossDie");
        }
    }

    public void ChangeState(BossState newState)
    {
        StopCoroutine(bossState.ToString());
        bossState = newState;
        StartCoroutine(bossState.ToString());
    }


    private IEnumerator Appear()
    {
        movement.Move(Vector3.up);

        while (true)
        {
            if (transform.position.y <= bossAppear)
            {
                movement.Move(Vector3.zero);
                ChangeState(BossState.Phase1);
            }
            yield return null;
        }

    }

    private IEnumerator Phase1()
    {

        yield return new WaitForSeconds(1.0f);
        animator.SetTrigger("IsAttack1");

        yield return new WaitForSeconds(1.0f);
        bossWeapon.StartFire(AtkType.Circle);
        while (true)
        { 
            if (CurrentHP <= MaxHP * 0.5f)
            {
                animator.SetTrigger("IsRun");
                ChangeState(BossState.Phase2);
            }
            yield return null;
        }
    }

    private IEnumerator Phase2()
    {
        //yield return new WaitForSeconds(1.0f);
        animator.SetTrigger("IsAttack2");
        yield return null;

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

    public IEnumerator BossDie()
    {
        isBossDie = true;
        // 보스 파괴 이펙트 생성
        //BossClearText.SetActive(true);

        yield return new WaitForSeconds(1.0f);


        animator.SetTrigger("IsDead");

        yield return new WaitForSeconds(1.0f);
        // 보스 오브젝트 삭제
        Destroy(gameObject);
    }
}
