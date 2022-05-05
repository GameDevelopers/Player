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
        // ��Ż UI �Ⱥ��̰� ����
        PortalUI.SetActive(false);
    }
    // ��Ż�� ������ �����ϰ� ���� �� �ؽ�Ʈ true
    void OnTriggerStay2D(Collider2D collision) {
        if ( collision.tag == "Player") {
            PortalUI.SetActive(true);            
        }   
        // ���� �� ����Ǹ� ��Ż ��Ȱ��ȭ
        if (collision.tag == "Player"&& SceneManager.GetActiveScene().name == "BossScene")
        {
            PortalUI.SetActive(false);
        }
    }
    // ��Ż�� �������� �ؽ�Ʈ false
    void OnTriggerExit2D(Collider2D collision) {
        if( collision.tag == "Player") {
            PortalUI.SetActive( false );
        }
    }
    void Update() {
        // ��ŻUI�� Ȱ��ȭ �Ǿ��� Ű���� ��Ű ���� �� �̵�
        // ��Ż ���� > ��Ż Ȱ��ȭ ��
        if (PortalUI.gameObject.activeSelf == true && Input.GetKeyDown(KeyCode.UpArrow)) {
            switch (Portal.gameObject.name) {
                case ("Portal1"):
                    // ���� ���� LoadScene("���弼�ÿ��� �̸�")
                    // �� �� ���� SceneManager.LoadScene("�̸�", LoadSceneMode.Additive);
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
