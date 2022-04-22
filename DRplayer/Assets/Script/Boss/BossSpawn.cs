using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    // ������� ����(���� ����� ����)
    //[SerializeField]
    //private AudioEffect audioEffect;
    // ���� ���� �ؽ�Ʈ ������Ʈ
    //[SerializeField]
    //private GameObject BossWarningText;
    /// <summary>
    /// ���� ������Ʈ
    /// </summary>
    [SerializeField]
    private GameObject boss;
    // ���� ü�� �г�
    [SerializeField]
    private GameObject panelBossHP;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        // ���� ���� �ؽ�Ʈ ��Ȱ
        //BossWarningText.SetActive(false);
        // ���� ü�� �г� ��Ȱ
        //panelBossHP.SetActive(false);
        // ���� ������Ʈ ��Ȱ
        //boss.SetActive(false);
        StartCoroutine("BossSpawnCor");
    }

    private IEnumerator BossSpawnCor()
    {
        // ���� ���� ��� ����
        //audioEffect.ChangeBgm(BGMType.Boss);
        // ���� ���� �ؽ�Ʈ Ȱ��
        //BossWarningText.SetActive(true);
        // 1�� ���
        yield return new WaitForSeconds(1.0f);

        // ���� ���� �ؽ�Ʈ ��Ȱ
        //BossWarningText.SetActive(false);
        // ���� ü�� �г� Ȱ��
        panelBossHP.SetActive(true);
        // ���� Ȱ��
        boss.SetActive(true);
        // ���� ������ ��ġ�� �̵�
        boss.GetComponent<Boss>().ChangeState(BossState.Appear);
    }
}
