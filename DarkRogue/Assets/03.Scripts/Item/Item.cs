using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// enum�Լ��� �̿��ؼ� ������ Ÿ�� ����
public enum ItemType { GEO, HP }

// �� óġ�� ����Ǵ� �������� �Ӽ��� �����ϴ� ��ũ��Ʈ
public class Item : MonoBehaviour
{
    [SerializeField]
    private ItemType itemType; // ������ Ÿ�� ����
    private float hp = 1f;  // ü�¾����ۿ� �ʿ��� ����

    // ������ ��� �� ���÷���(����)
    public Transform itemTransform; // �����۵��� ��ġ��
    private float delay = 0; 
    private float pasttime = 0;
    private float when = 1.0f;
    private Vector3 off; // �����۵��� ����� ���� ����

    // ���� ���� �ʱ�ȭ
    private void Awake()
    {
        // x���� ����
        off = new Vector3(Random.Range(-2, 2), off.y, off.z);
        // y���� ����
        off = new Vector3(off.x, Random.Range(-0.7f, -0.5f), off.z);
    }

    private void Update()
    {
        if(when >= delay)
        {
            pasttime = Time.deltaTime;
            itemTransform.position += off * Time.deltaTime;
            delay += pasttime;
        }
    }

    // �÷��̾�� �浹�� ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ������ Ÿ���� HP���
        if (itemType == ItemType.HP)
        {
            // �΋H�� �ݶ��̴��� �÷��̾� �±׸� �����ٸ�
            if (collision.tag == "Player")
            {
                // �±� �� Health ������Ʈ�� ü�� �߰�(1) �޼��带 ����
                collision.GetComponent<Health>().AddHealth(hp);
                // ������ �ı�
                Destroy(gameObject);
            }
        }
        // ������ Ÿ���� GEO���
        else if (itemType == ItemType.GEO)
        {
            // �΋H�� �ݶ��̴��� �÷��̾� �±׸� �����ٸ� 
            if (collision.tag == "Player")
            {
                // ������ �ı�
                Destroy(gameObject);
            }
        }
    }
}
