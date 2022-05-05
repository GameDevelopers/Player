using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OnlyIven : MonoBehaviour
{
    // 인벤토리 변수 선언
    Inventory inven;

    // 인벤토리 패널에 Inventory UI Panel 오브젝트를 담아준다.
    public GameObject inventoryPanel;

    // 활성화 여부 판단
    bool activeInventory = false;

    // 프리팹안의 슬롯들을 가져옴
    public Slot[] slots;
    public Transform slotHolder;

    // 일시정지 상태
    public bool isPause = false;


    private void Start()
    {
        // 인벤토리 변수 초기화
        inven = Inventory.instance;

        // GetComponentsInChildren이용해서 content안의 Slot 생성되는거 전부 선택
        slots = slotHolder.GetComponentsInChildren<Slot>();

        //onChangeItem이 참조할 메서드 정의
        inven.onChangeItem += RedrawSlotUI;

        // 한번 호출해서 모든 슬롯을 초기화
        RedrawSlotUI();

        // 초기에 인벤토리 안켜진 상태로 시작
        inventoryPanel.SetActive(activeInventory);
    }

    private void RedrawSlotUI()
    {
        // 반복문을 통해 슬롯들을 초기화
        // items의 갯수만큼 slot을 채워넣습니다.
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveSlot();
        }

        for (int i = 0; i < inven.items.Count; i++)
        {
            slots[i].item = inven.items[i];
            slots[i].UppdateSlotUI();
        }
    }
    private void Update()
    {
        // I 키로 인벤토리 창 활성화 + 상점이 열려있지 않다면
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!isPause)
            {
                SoundManager.sm.UIonoffPlay();
                // 일시정지 중이 아니면 일시정지
                Time.timeScale = 0; // 시간정지
                activeInventory = !activeInventory;
                inventoryPanel.SetActive(activeInventory); // 패널활성화
            }
            else
            {
                SoundManager.sm.UIonoffPlay();
                Time.timeScale = 1.0f; // 시간흐름 비율 1
                activeInventory = !activeInventory;
                inventoryPanel.SetActive(activeInventory);
            }

            isPause = !isPause; // 누를 때마다 상태가 바뀜
        }
    }
}
