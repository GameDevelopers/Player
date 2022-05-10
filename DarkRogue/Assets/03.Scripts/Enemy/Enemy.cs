using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적(Enemy) 프리팹에 관여하는 스크립트
public class Enemy : MonoBehaviour
{
    // 적 스프라이트
    private SpriteRenderer enemySprite;
    // 적 체력
    public int hp = 3;
    // 적 공격력
    public int enemyDamage = 1;
    // 적이 움직이는 거리
    [SerializeField]
    private float moveDistance;
    // 적이 움직이는 속도
    [SerializeField]
    private float speed;
    // 왼쪽으로 움직이는가
    private bool moveLeft;
    // 왼쪽 끝지점
    private float leftEdge;
    // 오른쪽 끝 지점
    private float rightEdge;
    private Animator enemyAnim;

    //적 관련 오디오
    public AudioSource audioSource;
    public AudioClip enemyClip;
    public AudioClip enemyDieClip;

    // 아이템 배열(돈, 체력)
    [SerializeField]
    private GameObject[] itemPrefabs;

    // 참조 및 초기화 메서드
    private void Awake()
    {
        enemySprite = GetComponent<SpriteRenderer>();
        enemyAnim = GetComponent<Animator>();
        // 왼쪽 끝 지점은 -x로 움직이는 거리만큼
        leftEdge = transform.position.x - moveDistance;
        // 오른쪽 끝 지점은 +x로 움직이는 거리만큼
        rightEdge = transform.position.x + moveDistance;
    }

    // 적의 움직임에 관여하는 Update메서드
    void Update()
    {
        // 만약 왼쪽으로 움직일 때
        if (moveLeft)
        {
            // 왼쪽 끝지점으로 설정한 값보다 현재 위치의 x값이 크면
            if (transform.position.x > leftEdge)
            {
                // 적 오디오 실행
                audioSource.PlayOneShot(enemyClip);
                // 적 스프라이트 반전 x
                enemySprite.flipX = false;

                // 위치 값을 현재위치 값에서 프레임에 위치한 x값을 뺀 값으로 변경
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            // 그 외
            else
            {
                // 왼쪽움직임 x
                moveLeft = false;
            }
        }
        // 그 외
        else
        {
            // 오른쪽 끝지점으로 설정한 값이 현재 위치의 x값보다 크면
            if (transform.position.x < rightEdge)
            {
                // 적 스프라이트 반전
                enemySprite.flipX = true;
                // 위치 값을 현재위치 값에서 프레임에 위치한 x값을 더한 값으로 변경
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            // 그 외
            else
            {
                // 왼쪽으로 움직임.
                moveLeft = true;
            }
        }
    }

    // 적 체력 감소 메서드
    public void HitDamage(int damage)
    {
        // 체력값은 damage변수 값 만큼(1)
        hp = hp - damage;
        // 적 체력이 0보다 작거나 같다면
        if (hp <= 0)
        {
            // 적 수 -1
            EnemySpawn.instance.enemyCount--;
            // 
            EnemySpawn.instance.isSpawn[int.Parse(transform.parent.name) - 1] = false;
            // 아이템 드랍 메서드 실행
            ItemSpawn();
            // 적 죽는 코루틴 실행
            StartCoroutine("EnemyDie");
        }
    }


    // 플레이어와 충돌하면
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 부딫힌 오브젝트의 태그가 플레이어면
        if (collision.tag == "Player")
        {
            // 적 공격력만큼 플레이어 체력감소
            collision.GetComponent<Health>().TakeDamage(enemyDamage);
        }
    }

    // 아이템 드랍 메서드
    private void ItemSpawn()
    {
        //  체력(10%) - 테스트용(100%), 지오(100%)
        int itemSpawn = Random.Range(0, 100);
        // 30%의 확률로
        if (itemSpawn < 30)
        {
            // 체력[0], 돈[1] 드랍
            Instantiate(itemPrefabs[0], transform.position, Quaternion.identity);
            Instantiate(itemPrefabs[1], transform.position, Quaternion.identity);
        }
        // 40%의 확률로
        else if (itemSpawn < 70)
        {
            // 돈[1], [2] 드랍
            Instantiate(itemPrefabs[1], transform.position, Quaternion.identity);
            Instantiate(itemPrefabs[2], transform.position, Quaternion.identity);
        }
        // 30%의 확률로
        else if (itemSpawn < 100)
        {
            // 돈[1], [2], [3] 드랍
            Instantiate(itemPrefabs[1], transform.position, Quaternion.identity);
            Instantiate(itemPrefabs[2], transform.position, Quaternion.identity);
            Instantiate(itemPrefabs[3], transform.position, Quaternion.identity);
        }
    }

    // 적 죽는 코루틴
    private IEnumerator EnemyDie()
    {
        // isDead파라미터 on
        enemyAnim.SetTrigger("IsDead");
        // 죽는 소리 play
        audioSource.PlayOneShot(enemyDieClip);
        // 적 콜라이더 비활성화(죽어도 콜라이더가 남아있으면 안됨)
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        // 일정 시간 후
        yield return new WaitForSeconds(0.5f);
        // 적 오브젝트 삭제
        Destroy(this.gameObject);
    }

}
