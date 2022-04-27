using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    // 배경음악 설정(보스 등장시 변경)
    //[SerializeField]
    //private AudioEffect audioEffect;
    // 보스 등장 텍스트 오브젝트
    //[SerializeField]
    //private GameObject BossWarningText;
    /// <summary>
    /// 보스 오브젝트
    /// </summary>
    [SerializeField]
    private GameObject boss;
    // 보스 체력 패널
    [SerializeField]
    private GameObject panelBossHP;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        // 보스 등장 텍스트 비활
        //BossWarningText.SetActive(false);
        // 보스 체력 패널 비활
        //panelBossHP.SetActive(false);
        // 보스 오브젝트 비활
        //boss.SetActive(false);
        StartCoroutine("BossSpawnCor");
    }

    private IEnumerator BossSpawnCor()
    {
        // 보스 등장 브금 설정
        //audioEffect.ChangeBgm(BGMType.Boss);
        // 보스 등장 텍스트 활성
        //BossWarningText.SetActive(true);
        // 1초 대기
        yield return new WaitForSeconds(1.0f);

        // 보스 등장 텍스트 비활
        //BossWarningText.SetActive(false);
        // 보스 체력 패널 활성
        panelBossHP.SetActive(true);
        // 보스 활성
        boss.SetActive(true);
        // 보스 지정된 위치로 이동
        boss.GetComponent<Boss>().ChangeState(BossState.Appear);
    }
}
