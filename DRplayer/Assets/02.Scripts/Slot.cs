using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerUpHandler
{
    // 정수형 변수 생성
    public int slotnum;

    // 아이템 , 이미지 변수 생성
    public InvenItem item;
    public Image itemIcon;

    // 참이면 판매모드, 거짓이면 아이템 사용모드
    public bool isShopMode;
    // 아이템이 선택되면 참으로 바뀌어서 ok눌렀을 때 참인 아이템만 판매되도록 설정
    public bool isSell = false;
    // isSell이 참이면 체크표시 아이콘을 만들어준다.
    public GameObject chkSell;

    public void UppdateSlotUI()
    {
        // itemIcon sprite를 아이템 이미지로 초기화하고 활성화 시켜준다.
        itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);

    }

    // item은 null로 SetActive는 false를 시켜준다
    public void RemoveSlot()
    {
        item = null;
        itemIcon.gameObject.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(item != null)
        {
            if (!isShopMode)
            {
                // Slot에 있는 item.Use메서드를 호출합니다.
                bool isUse = item.Use();
                // 아이템 사용에 성공하면 RemoveItem을 호출
                if (isUse)
                {
                    // Inventory의 items에서 알맞은 속성을 제거
                    Inventory.instance.RemoveItem(slotnum);
                }
            }
            else
            {
                // 상점
                isSell = true;
                chkSell.SetActive(isSell);
            }
        }
    }

    // 판매할 아이템
    public void SellItem()
    {   // isSell 이 참이면 아이템DB의 머니 변수에 아이템의 비용만큼 증가시킨다
        if (isSell)
        {
            ItemDataBase.instance.money += item.itemCost; // 아이템 스크립트에 변수 작성
            Inventory.instance.RemoveItem(slotnum); // 같은 번호의 아이템을 제거
            // isSell 초기화
            isSell = false;
            chkSell.SetActive(isSell);
        }
    }

    // 판매를 안하고 상점을 종료하면 선택된 아이템 해제
    private void OnDisable()
    {
        isSell = false;
        chkSell.SetActive(isSell);
    }
}
