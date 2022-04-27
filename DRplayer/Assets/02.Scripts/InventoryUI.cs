using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
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

    // 인벤토리 슬롯 가져온 방식과동일
    public ShopSlot[] shopSlots;
    public Transform shopHolder;

    private void Start()
    {
        // 인벤토리 변수 초기화
        inven = Inventory.instance;

        // GetComponentsInChildren이용해서 content안의 Slot 생성되는거 전부 선택
        slots = slotHolder.GetComponentsInChildren<Slot>();
        // 위와 동일한 방식
        shopSlots = shopHolder.GetComponentsInChildren<ShopSlot>();

        // 자기자신을 인자로 넘겨준다
        for (int i = 0; i < shopSlots.Length; i++)
        {
            shopSlots[i].Init(this);
            // 상점 슬롯의 몇번째 슬롯인지 지정해준다.
            shopSlots[i].slotnum = i;
        }

        // onSlotCountChange가 참조할 메서드 정의
        //inven.onSlotCountChange += SlotChange;
        // 대리자 호출보다 메서드가 먼저 실행되는 에러 발생 시 
        // Edit - project Settings - Script Execution Order - InventoryUI , Inventory 순으로 
        // 스크립트 순서 지정해서 해결


        //onChangeItem이 참조할 메서드 정의
        inven.onChangeItem += RedrawSlotUI;

        // 한번 호출해서 모든 슬롯을 초기화
        RedrawSlotUI();

        // 초기에 인벤토리 안켜진 상태로 시작
        inventoryPanel.SetActive(activeInventory);

        // 상점 비활성화 닫기 버튼 
        closeShop.onClick.AddListener(DeActiveShop);
    }


    private void SlotChange(int val)
    {
        // slots의 갯수만큼만 활성화 하고 나머지는 비활성화
        for (int i = 0; i < slots.Length; i++)
        {
            // Slot의 slotnum을 차례로 번호를 부여 
            slots[i].slotnum = i;

            // SlotCnt 만큼만 interactable 를 true 줘서 활성화
            // Button 의 interactable이 false면 비활성화된다.
            if (i < inven.SlotCnt)
            {
                slots[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                slots[i].GetComponent<Button>().interactable = false;
            }
        }
    }
    // 슬롯개수 증가
    public void AddSlot()
    {
        inven.SlotCnt++;
    }

    private void Update()
    {
        // I 키로 인벤토리 창 활성화 + 상점이 열려있지 않다면
        if (Input.GetKeyDown(KeyCode.I) && !isStoreActive)
        {
            activeInventory = !activeInventory;
            inventoryPanel.SetActive(activeInventory);
        }
        // 마우스 버튼이 클릭 되면 rayshop에서 메소드 호출
        if (Input.GetMouseButtonUp(0))
        {
            RayShop();
        }
    }

    //
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



    // 상점 클릭 시 거래창 활성화 되게하는 코드 작성
    public GameObject shop;
    // 상점을 비활성화 시키는 코드 작성을 위한 버튼변수
    public Button closeShop;
    // 이 변수가 참이면 i키를 눌러도 인벤이 비활성화 되지 않게 막아준다.
    public bool isStoreActive;
    // 상점정보를 넣어두는 변수
    public ShopData shopData;

    //  클릭한 위치에 레이를 쏴 상점 위치를 체크
    public void RayShop()
    {
        // 마우스의 위치를 가져와서 화면 좌표를 월드좌표로 반환 받아 사용
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = -10;
        // 확인 DrawRay 씬 창에 레이를 그려줌
        Debug.DrawRay(mousePos, transform.forward, Color.red, 0.5f);

        // 레이가 ui를 뚫고 가지 못하도록 만들어준다.
        // 모바일은 -1 대신 0
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1))
        {
            // 레이캐스트를 이용해서 히트값을 가져온다.
            RaycastHit2D hit2D = Physics2D.Raycast(mousePos, transform.forward, 30);
            if (hit2D.collider != null)
            { // hit값이 비어있지 않고
                // 충돌체의 태그가 스토어라면
                if (hit2D.collider.CompareTag( "Store"))
                {
                    if (!isStoreActive)
                    {
                        ActiveShop(true);

                        //상점을 클릭시 샵데이터를 가져오고
                        shopData = hit2D.collider.GetComponent<ShopData>();
                        // 상점슬롯에 아이템을 넣어주고 UI를 보여준다.
                        for (int i = 0; i < shopData.stocks.Count; i++)
                        {
                            //Debug.Log(shopData.stocks.Count);
                            // 상점의 아이템 데이터를 받아서 상점 슬롯에 채워준다.
                            shopSlots[i].item = shopData.stocks[i];
                            shopSlots[i].UppdateSlotUI();

                            //Debug.Log(shopData.stocks[i]);
                        }
                    }
                    
                }
            }
        }
    }

    // 슬롯에 번호를 넘겨받을 수 있게 해주는 정수형 변수 받기
    public void Buy(int num)
    {
        // 상점에 몇번째 아이템이 팔렸는지 정보를 알려준다.
        shopData.soldOuts[num] = true;
    }

    // 비/활성화 코드는 따로 작성
    public void ActiveShop(bool isOpen)
    {
        if (!activeInventory)
        {
            // 인벤토리 활성여부 : 상점이 켜지면 i키를 눌러도 인벤토리 창이 사라지지않고,
            // 인벤토리 창이 켜지면 상점이 안켜지게 설정
            isStoreActive = isOpen;
            shop.SetActive(isOpen);
            inventoryPanel.SetActive(!isOpen); // 원래 isOpen
            // 모든 슬롯의 샵모드를 매개변수에다가 초기화되게 만들어준다
            for (int i = 0; i < slots.Length; i++)
            {   // 전달인자를 참으로 전달하게 되면 모든 슬롯에 샵모드가 참이된다
                // 반대로 close 버튼을 누르면 샵모드가 전부 거짓이 된다.
                slots[i].isShopMode = isOpen;
            }
        }

    }

    // 상점창 끄는 메서드
    public void DeActiveShop()
    {
        ActiveShop(false);
        //  샵데이터와 연결을 끊어주고 상점의 슬롯을 전부 초기화 시킨다.
        shopData = null;
        for (int i = 0; i < shopSlots.Length; i++)
        {
            shopSlots[i].RemoveSlot();
            inventoryPanel.SetActive(false); // 추가
        }
    }

    //
    public void SellBtn()
    {
        // 슬롯의 크기에서 하나씩 빼면서 진행
        // 0부터 시작하면 뒷부분의 슬롯이 앞으로 밀려나면서 데이터가 꼬이기 떄문이다
        for (int i = slots.Length; i > 0; i--)
        {
            slots[i - 1].SellItem();
        }
    }
}
