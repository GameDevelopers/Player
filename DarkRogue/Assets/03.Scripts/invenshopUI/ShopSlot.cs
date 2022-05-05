using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


//  Slot.cs에서 UppdateSlotUI,RemoveSlot,OnPointerUp(얘만 내용삭제) 
public class ShopSlot : MonoBehaviour, IPointerUpHandler
{
    public int slotnum;
    public InvenItem item;
    public Image itemIcon;
    // 아이템이 팔렸음을 표시
    public bool soldOut = false;
    // 인벤토리 ui에 메서드를 호출
    InventoryUI inventoryUI;

    //인벤토리 ui에 메서드를 호출 
    public void Init(InventoryUI Iui)
    {
        inventoryUI = Iui;
    }


    public void UppdateSlotUI()
    {
        // itemIcon sprite를 아이템 이미지로 초기화하고 활성화 시켜준다.

        Debug.Log("UppdateSlotUI");

        itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);
        // 아이템이 팔렸다면 어둡게 만들어주는 코드 
        if (soldOut)
        {
            itemIcon.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    // item은 null로 SetActive는 false를 시켜준다
    public void RemoveSlot()
    {
        item = null;
        // 솔드아웃이 false로 초기화
        soldOut = false;
        itemIcon.gameObject.SetActive(false);
    }

    // 상점 슬롯이 눌리면 소지한 돈과 아이템의 가격을 비교해서 돈이 부족하지 않다면
    // 아이템을 구매시 가격만큼 돈에서 차감하고 해당아이템을 인벤토리에 표시
    public void OnPointerUp(PointerEventData eventData)
    {
        if (item != null)
        {                                                                    // 인벤이 꽉 차지 않았을 때만 아이템이 구매 가능하도록
            if (ItemDataBase.instance.money >= item.itemCost && !soldOut && Inventory.instance.items.Count < Inventory.instance.SlotCnt)
            {
                ItemDataBase.instance.money -= item.itemCost;
                Inventory.instance.AddItem(item);
                // 슬롯이 클릭되면 UI의 Buy메서드를 호출하고 soldOut을 참으로 변경 
                soldOut = true;
                inventoryUI.Buy(slotnum);
                UppdateSlotUI(); // 구매가 끝나면 업데이트 UI로 슬롯을 다시 그려준다.
            }
        }
    }

}
