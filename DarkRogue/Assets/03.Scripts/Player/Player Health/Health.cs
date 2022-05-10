using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 플레이어 체력에 관여하는 스크립트
public class Health : MonoBehaviour
{
    [SerializeField]
    private float startingHealth; // 시작 체력
    public float currentHealth { get; private set; } // 현재 체력 ( 다른 스크립트에서 변경 불가, 가져가는 건 가능)
    private Animator anim;

    // 초기화 및 참조 메서드
    private void Awake()
    {
        // 현재 체력을 시작 체력으로 초기화
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    // 플레이어 피격 메서드(damage만큼)
    public void TakeDamage(float damage)
    {
        // 현재 체력 = 최소값과 최대값 사이의 값을 고정하고 값을 반환
        // 현재체력에서 damage만큼 감소시킴. 단, 체력은 0 ~ 시작 체력)
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        // 만약 현재 체력이 0보다 크면
        if (currentHealth > 0)
        {
            // 플레이어 Hurt
            // IsHurt파라미터 발동
            anim.SetTrigger("IsHurt");

        }
        // 그외
        else
        {
            // 플레이어 죽음.
            // Die코루틴 실행
            StartCoroutine("Die");
        }
    }

    // 플레이어 체력 증가 메서드(Hp만큼)
    public void AddHealth(float Hp)
    {
        // 현재체력에서 Hp만큼 증가시킴. 단, 체력은 0 ~ 시작 체력)
        currentHealth = Mathf.Clamp(currentHealth + Hp, 0, startingHealth);
    }

    // 플레이어 죽는 코루틴
    private IEnumerator Die()
    {
        // IsDead 파라미터 발동
        anim.SetTrigger("IsDead");
        // 일정 시간 후
        yield return new WaitForSeconds(1f);
        // 현재 맵의 처음으로 돌아가기(로그라이크 게임 특징)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 장애물 충돌
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 만약 부딫힌 태그가 Spark라면
        if (collision.tag == "Spark")
        {
            // 장애물 충돌 시 체력 -1
            collision.GetComponent<Health>().TakeDamage(1.0f);
        }
    }

}
