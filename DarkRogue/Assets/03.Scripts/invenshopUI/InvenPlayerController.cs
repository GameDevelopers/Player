using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenPlayerController : MonoBehaviour
{
    // 말하는 중인지 확인
    public bool talk = false;
    // 대사를 담아놓을 배열
    public string[] sentences;
    // 마지막으로 대사를 한 시간
    float lastSpaceTime = 0f;
    // 공백시간 주기, 대화 넘기기 연달아서 누르기 방지
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
            // 알파값 조절로 대화창 활성화
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
