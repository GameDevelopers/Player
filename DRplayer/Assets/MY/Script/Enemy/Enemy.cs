using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D enemyRb;
    private SpriteRenderer enemySprite;
    // �� ü��
    public int hp = 3;
    // �� ���ݷ�
    public int enemyDamage = 1;
    // ���� �����̴� �Ÿ�
    [SerializeField]
    private float moveDistance;
    // ���� �����̴� �ӵ�
    [SerializeField]
    private float speed;
    // �������� �����̴°�
    private bool moveLeft;
    // ���� ������
    private float leftEdge;
    // ������ �� ����
    private float rightEdge;
    private Animator enemyAnim;

    // ������ ��� �迭(����, ȸ��)
    [SerializeField]
    private GameObject[] itemPrefabs;

    private void Awake()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        enemySprite = GetComponent<SpriteRenderer>();
        enemyAnim = GetComponent<Animator>();
        // ���� �� ������ -x�� �����̴� �Ÿ���ŭ
        leftEdge = transform.position.x - moveDistance;
        // ������ �� ������ +x�� �����̴� �Ÿ���ŭ
        rightEdge = transform.position.x + moveDistance;
    }

    void Update()
    {
       

        // �� �¿�� ������.
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

    // �� ü�� ����
    public void HitDamage(int damage)
    {
        hp = hp - damage;
        // �� ü���� 0���� �۰ų� ���ٸ�
        if (hp <= 0)
        {
            // �� �� -1
            EnemySpawn.instance.enemyCount--;
            EnemySpawn.instance.isSpawn[int.Parse(transform.parent.name) - 1] = false;
            ItemSpawn();
            //enemyAnim.SetTrigger("IsDead");
            //Destroy(this.gameObject);

            StartCoroutine("EnemyDie");
        }
    }


    // �÷��̾�� �浹�ϸ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // �� ���ݷ¸�ŭ �÷��̾� ü�°���
            collision.GetComponent<Health>().TakeDamage(enemyDamage);
        }
    }

    private void ItemSpawn()
    {
        //  ü��(10%) - �׽�Ʈ��(100%), ����(100%)
        int itemSpawn = Random.Range(0, 100);
        // ü��
        if (itemSpawn < 30)
        {
            Instantiate(itemPrefabs[0], transform.position, Quaternion.identity);
            Instantiate(itemPrefabs[1], transform.position, Quaternion.identity);
        }
        // ����
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
