using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// �÷��̾� ü�¿� �����ϴ� ��ũ��Ʈ
public class Health : MonoBehaviour
{
    [SerializeField]
    private float startingHealth; // ���� ü��
    public float currentHealth { get; private set; } // ���� ü�� ( �ٸ� ��ũ��Ʈ���� ���� �Ұ�, �������� �� ����)
    private Animator anim;

    // �ʱ�ȭ �� ���� �޼���
    private void Awake()
    {
        // ���� ü���� ���� ü������ �ʱ�ȭ
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    // �÷��̾� �ǰ� �޼���(damage��ŭ)
    public void TakeDamage(float damage)
    {
        // ���� ü�� = �ּҰ��� �ִ밪 ������ ���� �����ϰ� ���� ��ȯ
        // ����ü�¿��� damage��ŭ ���ҽ�Ŵ. ��, ü���� 0 ~ ���� ü��)
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        // ���� ���� ü���� 0���� ũ��
        if (currentHealth > 0)
        {
            // �÷��̾� Hurt
            // IsHurt�Ķ���� �ߵ�
            anim.SetTrigger("IsHurt");

        }
        // �׿�
        else
        {
            // �÷��̾� ����.
            // Die�ڷ�ƾ ����
            StartCoroutine("Die");
        }
    }

    // �÷��̾� ü�� ���� �޼���(Hp��ŭ)
    public void AddHealth(float Hp)
    {
        // ����ü�¿��� Hp��ŭ ������Ŵ. ��, ü���� 0 ~ ���� ü��)
        currentHealth = Mathf.Clamp(currentHealth + Hp, 0, startingHealth);
    }

    // �÷��̾� �״� �ڷ�ƾ
    private IEnumerator Die()
    {
        // IsDead �Ķ���� �ߵ�
        anim.SetTrigger("IsDead");
        // ���� �ð� ��
        yield return new WaitForSeconds(1f);
        // ���� ���� ó������ ���ư���(�α׶���ũ ���� Ư¡)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ��ֹ� �浹
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� �΋H�� �±װ� Spark���
        if (collision.tag == "Spark")
        {
            // ��ֹ� �浹 �� ü�� -1
            collision.GetComponent<Health>().TakeDamage(1.0f);
        }
    }

}
