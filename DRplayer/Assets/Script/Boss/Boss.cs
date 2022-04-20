using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BossState { MoveToAppear = 0, Phase1, Phase2}

public class Boss : MonoBehaviour
{
    public bool isBossDie = false;
    [SerializeField]
    private float bossAppear = 2.5f;
    private BossState bossState = BossState.MoveToAppear;
    private Movement movement;
    //private BossWeapon bossWeapon;
    private BossHP bossHP;

    //[SerializeField]
    //public GameObject BossClearText;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        //bossWeapon = GetComponent<BossWeapon>();
        bossHP = GetComponent<BossHP>();
    }

    public void ChangeState(BossState newState)
    {
        StopCoroutine(bossState.ToString());
        bossState = newState;
        StartCoroutine(bossState.ToString());
    }

    private IEnumerator MoveToAppear()
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
        yield return null;
    }

    private IEnumerator Phase2()
    {
        yield return null;
    }

    public void BossDie()
    {
        isBossDie = true;
        // 보스 파괴 이펙트 생성
        //BossClearText.SetActive(true);
        // 보스 오브젝트 삭제
        Destroy(gameObject);
    }
}
