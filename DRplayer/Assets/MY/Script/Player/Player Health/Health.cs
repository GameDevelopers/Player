using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            // 플레이어 Hurt
            anim.SetTrigger("IsHurt");

        }
        else
        {
            StartCoroutine("Die");
        }
    }

    public void AddHealth(float Hp)
    {
        currentHealth = Mathf.Clamp(currentHealth + Hp, 0, startingHealth);
    }

    private IEnumerator Die()
    {
        anim.SetTrigger("IsDead");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
