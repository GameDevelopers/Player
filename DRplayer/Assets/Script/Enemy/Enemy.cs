using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D enemyRb;
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

    // 아이템 드랍 배열(지오, 회복)
    [SerializeField]
    private GameObject[] itemPrefabs;

    private void Awake()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        enemySprite = GetComponent<SpriteRenderer>();
        enemyAnim = GetComponent<Animator>();
        // 왼쪽 끝 지점은 -x로 움직이는 거리만큼
        leftEdge = transform.position.x - moveDistance;
        // 오른쪽 끝 지점은 +x로 움직이는 거리만큼
        rightEdge = transform.position.x + moveDistance;
    }

    void Update()
    {
       

        // 적 좌우로 움직임.
        if (moveLeft)
        {
            if (transform.position.x > leftEdge)
            {
                enemySprite.flipX = false;

                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                moveLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightEdge)
            {
                enemySprite.flipX = true;

                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                moveLeft = true;
            }
        }
    }

    // 적 체력 감소
    public void HitDamage(int damage)
    {
        hp = hp - damage;
        // 적 체력이 0보다 작거나 같다면
        if (hp <= 0)
        {
            // 적 수 -1
            EnemySpawn.instance.enemyCount--;
            EnemySpawn.instance.isSpawn[int.Parse(transform.parent.name) - 1] = false;
            ItemSpawn();
            //enemyAnim.SetTrigger("IsDead");
            //Destroy(this.gameObject);

            StartCoroutine("EnemyDie");
        }
    }


    // 플레이어와 충돌하면
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // 적 공격력만큼 플레이어 체력감소
            collision.GetComponent<Health>().TakeDamage(enemyDamage);
        }
    }

    private void ItemSpawn()
    {
        //  체력(10%) - 테스트용(100%), 지오(100%)
        int itemSpawn = Random.Range(0, 100);
        // 체력
        if (itemSpawn < 30)
        {
            Instantiate(itemPrefabs[0], transform.position, Quaternion.identity);
            Instantiate(itemPrefabs[1], transform.position, Quaternion.identity);
        }
        // 지오
        else if (itemSpawn < 70)
        {
            Instantiate(itemPrefabs[1], transform.position, Quaternion.identity);
            Instantiate(itemPrefabs[2], transform.position, Quaternion.identity);
        }
        else if (itemSpawn < 100)
        {
            Instantiate(itemPrefabs[1], transform.position, Quaternion.identity);
            Instantiate(itemPrefabs[2], transform.position, Quaternion.identity);
            Instantiate(itemPrefabs[3], transform.position, Quaternion.identity);
        }
    }

    private IEnumerator EnemyDie()
    {
        enemyAnim.SetTrigger("IsDead");
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

}
