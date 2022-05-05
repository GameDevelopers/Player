using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenPlayerController : MonoBehaviour
{
    // ���ϴ� ������ Ȯ��
    public bool talk = false;
    // ��縦 ��Ƴ��� �迭
    public string[] sentences;
    // ���������� ��縦 �� �ð�
    float lastSpaceTime = 0f;
    // ����ð� �ֱ�, ��ȭ �ѱ�� ���޾Ƽ� ������ ����
    float spaceTime = 0.5f;

    
    private void Update()
    {
        if(talk) NPCTalk();

    }

    void NPCTalk()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastSpaceTime + spaceTime)
        {
            lastSpaceTime = Time.time;
            // ���İ� ������ ��ȭâ Ȱ��ȭ
            switch (DialogManager.instance.dialoggroup.alpha)
            {
                case 0:
                    if (!DialogManager.instance.istyping)
                    {
                        DialogManager.instance.Ondialog(sentences);
                    }
                    break;
                case 1:
                    if(DialogManager.instance.sentences.Count != 0)
                    {
                        DialogManager.instance.NextSentence();
                    }
                    else
                    {
                        if (DialogManager.instance.jump) DialogManager.instance.NextSentence();
                        else DialogManager.instance.jump = true;
                        //DialogManager.instance.jump = true;
                    }
                    break;
                default:
                    Debug.Log("Error");
                    break;
            }
        }
    }


}
