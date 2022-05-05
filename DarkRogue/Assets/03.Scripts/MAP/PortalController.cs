using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    // Hierarchy Portal UI Text, Portal
    public GameObject PortalUI;
    public GameObject Portal;

    private void Awake() {
        // 포탈 UI 안보이게 시작
        PortalUI.SetActive(false);
    }
    // 포탈이 유저와 접촉하고 있을 시 텍스트 true
    void OnTriggerStay2D(Collider2D collision) {
        if ( collision.tag == "Player") {
            PortalUI.SetActive(true);            
        }   
        // 보스 씬 진행되면 포탈 비활성화
        if (collision.tag == "Player"&& SceneManager.GetActiveScene().name == "BossScene")
        {
            PortalUI.SetActive(false);
        }
    }
    // 포탈과 떨어지면 텍스트 false
    void OnTriggerExit2D(Collider2D collision) {
        if( collision.tag == "Player") {
            PortalUI.SetActive( false );
        }
    }
    void Update() {
        // 포탈UI가 활성화 되었고 키보드 윗키 누를 시 이동
        // 포탈 접촉 > 포탈 활성화 시
        if (PortalUI.gameObject.activeSelf == true && Input.GetKeyDown(KeyCode.UpArrow)) {
            switch (Portal.gameObject.name) {
                case ("Portal1"):
                    // 완전 변경 LoadScene("빌드세팅에서 이름")
                    // 새 씬 열기 SceneManager.LoadScene("이름", LoadSceneMode.Additive);
                    SoundManager.sm.PortalclipPlay();
                    SceneManager.LoadScene("VillScene");
                    break;
                case ("Portal2"):
                    SoundManager.sm.PortalclipPlay();
                    SceneManager.LoadScene("MapScene");
                    break;
                case ("Portal3"):
                    SoundManager.sm.PortalclipPlay();
                    SceneManager.LoadScene("BossScene");
                    break;
                case ("Portal4"):
                    SoundManager.sm.PortalclipPlay();
                    SceneManager.LoadScene("Shop");
                    break;
            }
        }
    }
}
