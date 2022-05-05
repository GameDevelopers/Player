using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject menuPanel; // 메뉴 클릭시 패널 활성화
    public bool isPause = false; // 일시정지 상태
    public GameObject Keymap; // 키맵 패널
    public bool isOpen = false; // 키맵 열린지 확인

    public void Startgame()
    {
        
         SoundManager.sm.SelectBtnPlay();
         StartCoroutine(SceneChange());
         //SceneManager.LoadScene(1);  
    }

    IEnumerator SceneChange()
    {
        yield return new WaitForSeconds(1.5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void HomeMenu()
    {
        SoundManager.sm.HomeBtnPlay();
        SceneManager.LoadScene(0);
    }

    public void Exitgame()
    {
         Application.Quit();
    }

    // 메뉴 클릭시 화면 일시정지 및 패널 활성화
    public void OnMenu()
    {
        if (!isPause)
        {
            SoundManager.sm.UIonoffPlay(); // 사운드
            // 일시정지 중이 아니면 일시정지
            Time.timeScale = 0; // 시간정지
            // 패널활성화
            menuPanel.SetActive(true);
        }
        else
        {
            SoundManager.sm.UIonoffPlay();
            Time.timeScale = 1.0f; // 시간흐름 비율 1
            menuPanel.SetActive(false);
        }

        isPause = !isPause; // 메뉴 누를 때마다 상태가 반대로 바뀜
    } 
    
    public void OnKeymap()
    {
        if (!isOpen)
        {
            SoundManager.sm.UIonoffPlay(); // 사운드
            menuPanel.SetActive(false);
            // 키맵 활성화
            Keymap.SetActive(true);
            
        }

        else
        {
             SoundManager.sm.UIonoffPlay();
             Keymap.SetActive(false);
             Time.timeScale = 1.0f; // 시간흐름 비율 1
        }

        isOpen = !isOpen;
        
    }





}
