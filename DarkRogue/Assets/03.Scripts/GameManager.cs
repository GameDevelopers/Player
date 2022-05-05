using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject menuPanel; // �޴� Ŭ���� �г� Ȱ��ȭ
    public bool isPause = false; // �Ͻ����� ����
    public GameObject Keymap; // Ű�� �г�
    public bool isOpen = false; // Ű�� ������ Ȯ��

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

    // �޴� Ŭ���� ȭ�� �Ͻ����� �� �г� Ȱ��ȭ
    public void OnMenu()
    {
        if (!isPause)
        {
            SoundManager.sm.UIonoffPlay(); // ����
            // �Ͻ����� ���� �ƴϸ� �Ͻ�����
            Time.timeScale = 0; // �ð�����
            // �г�Ȱ��ȭ
            menuPanel.SetActive(true);
        }
        else
        {
            SoundManager.sm.UIonoffPlay();
            Time.timeScale = 1.0f; // �ð��帧 ���� 1
            menuPanel.SetActive(false);
        }

        isPause = !isPause; // �޴� ���� ������ ���°� �ݴ�� �ٲ�
    } 
    
    public void OnKeymap()
    {
        if (!isOpen)
        {
            SoundManager.sm.UIonoffPlay(); // ����
            menuPanel.SetActive(false);
            // Ű�� Ȱ��ȭ
            Keymap.SetActive(true);
            
        }

        else
        {
             SoundManager.sm.UIonoffPlay();
             Keymap.SetActive(false);
             Time.timeScale = 1.0f; // �ð��帧 ���� 1
        }

        isOpen = !isOpen;
        
    }





}
