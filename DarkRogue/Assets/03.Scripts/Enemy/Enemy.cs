using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��(Enemy) �����տ� �����ϴ� ��ũ��Ʈ
public class Enemy : MonoBehaviour
{
    // �� ��������Ʈ
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

    //�� ���� �����
    public AudioSource audioSource;
    public AudioClip enemyClip;
    public AudioClip enemyDieClip;

    // ������ �迭(��, ü��)
    [SerializeField]
    private GameObject[] itemPrefabs;

    // ���� �� �ʱ�ȭ �޼���
    private void Awake()
    {
        enemySprite = GetComponent<SpriteRenderer>();
        enemyAnim = GetComponent<Animator>();
        // ���� �� ������ -x�� �����̴� �Ÿ���ŭ
        leftEdge = transform.position.x - moveDistance;
        // ������ �� ������ +x�� �����̴� �Ÿ���ŭ
        rightEdge = transform.position.x + moveDistance;
    }

    // ���� �����ӿ� �����ϴ� Update�޼���
    void Update()
    {
        // ���� �������� ������ ��
        if (moveLeft)
        {
            // ���� ���������� ������ ������ ���� ��ġ�� x���� ũ��
            if (transform.position.x > leftEdge)
            {
                // �� ����� ����
                audioSource.PlayOneShot(enemyClip);
                // �� ��������Ʈ ���� x
                enemySprite.flipX = false;

                // ��ġ ���� ������ġ ������ �����ӿ� ��ġ�� x���� �� ������ ����
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            // �� ��
            else
            {
                // ���ʿ����� x
                moveLeft = false;
            }
        }
        // �� ��
        else
        {
            // ������ ���������� ������ ���� ���� ��ġ�� x������ ũ��
            if (transform.position.x < rightEdge)
            {
                // �� ��������Ʈ ����
                enemySprite.flipX = true;
                // ��ġ ���� ������ġ ������ �����ӿ� ��ġ�� x���� ���� ������ ����
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            // �� ��
            else
            {
                // �������� ������.
                moveLeft = true;
            }
        }
    }

    // �� ü�� ���� �޼���
    public void HitDamage(int damage)
    {
        // ü�°��� damage���� �� ��ŭ(1)
        hp = hp - damage;
        // �� ü���� 0���� �۰ų� ���ٸ�
        if (hp <= 0)
        {
            // �� �� -1
            EnemySpawn.instance.enemyCount--;
            // 
            EnemySpawn.instance.isSpawn[int.Parse(transform.parent.name) - 1] = false;
            // ������ ��� �޼��� ����
            ItemSpawn();
            // �� �״� �ڷ�ƾ ����
            StartCoroutine("EnemyDie");
        }
    }


    // �÷��̾�� �浹�ϸ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �΋H�� ������Ʈ�� �±װ� �÷��̾��
        if (collision.tag == "Player")
        {
            // �� ���ݷ¸�ŭ �÷��̾� ü�°���
            collision.GetComponent<Health>().TakeDamage(enemyDamage);
        }
    }

    // ������ ��� �޼���
    private void ItemSpawn()
    {
        //  ü��(10%) - �׽�Ʈ��(100%), ����(100%)
        int itemSpawn = Random.Range(0, 100);
        // 30%�� Ȯ����
        if (itemSpawn < 30)
        {
            // ü��[0], ��[1] ���
            Instantiate(itemPrefabs[0], transform.position, Quaternion.identity);
            Instantiate(itemPrefabs[1], transform.position, Quaternion.identity);
        }
        // 40%�� Ȯ����
        else if (itemSpawn < 70)
        {
            // ��[1], [2] ���
            Instantiate(itemPrefabs[1], transform.position, Quaternion.identity);
            Instantiate(itemPrefabs[2], transform.position, Quaternion.identity);
        }
        // 30%�� Ȯ����
        else if (itemSpawn < 100)
        {
            // ��[1], [2], [3] ���
            Instantiate(itemPrefabs[1], transform.position, Quaternion.identity);
            Instantiate(itemPrefabs[2], transform.position, Quaternion.identity);
            Instantiate(itemPrefabs[3], transform.position, Quaternion.identity);
        }
    }

    // �� �״� �ڷ�ƾ
    private IEnumerator EnemyDie()
    {
        // isDead�Ķ���� on
        enemyAnim.SetTrigger("IsDead");
        // �״� �Ҹ� play
        audioSource.PlayOneShot(enemyDieClip);
        // �� �ݶ��̴� ��Ȱ��ȭ(�׾ �ݶ��̴��� ���������� �ȵ�)
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        // ���� �ð� ��
        yield return new WaitForSeconds(0.5f);
        // �� ������Ʈ ����
        Destroy(this.gameObject);
    }

}
