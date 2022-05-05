using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


//  Slot.cs���� UppdateSlotUI,RemoveSlot,OnPointerUp(�길 �������) 
public class ShopSlot : MonoBehaviour, IPointerUpHandler
{
    public int slotnum;
    public InvenItem item;
    public Image itemIcon;
    // �������� �ȷ����� ǥ��
    public bool soldOut = false;
    // �κ��丮 ui�� �޼��带 ȣ��
    InventoryUI inventoryUI;

    //�κ��丮 ui�� �޼��带 ȣ�� 
    public void Init(InventoryUI Iui)
    {
        inventoryUI = Iui;
    }


    public void UppdateSlotUI()
    {
        // itemIcon sprite�� ������ �̹����� �ʱ�ȭ�ϰ� Ȱ��ȭ �����ش�.

        Debug.Log("UppdateSlotUI");

        itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);
        // �������� �ȷȴٸ� ��Ӱ� ������ִ� �ڵ� 
        if (soldOut)
        {
            itemIcon.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    // item�� null�� SetActive�� false�� �����ش�
    public void RemoveSlot()
    {
        item = null;
        // �ֵ�ƿ��� false�� �ʱ�ȭ
        soldOut = false;
        itemIcon.gameObject.SetActive(false);
    }

    // ���� ������ ������ ������ ���� �������� ������ ���ؼ� ���� �������� �ʴٸ�
    // �������� ���Ž� ���ݸ�ŭ ������ �����ϰ� �ش�������� �κ��丮�� ǥ��
    public void OnPointerUp(PointerEventData eventData)
    {
        if (item != null)
        {                                                                    // �κ��� �� ���� �ʾ��� ���� �������� ���� �����ϵ���
            if (ItemDataBase.instance.money >= item.itemCost && !soldOut && Inventory.instance.items.Count < Inventory.instance.SlotCnt)
            {
                ItemDataBase.instance.money -= item.itemCost;
                Inventory.instance.AddItem(item);
                // ������ Ŭ���Ǹ� UI�� Buy�޼��带 ȣ���ϰ� soldOut�� ������ ���� 
                soldOut = true;
                inventoryUI.Buy(slotnum);
                UppdateSlotUI(); // ���Ű� ������ ������Ʈ UI�� ������ �ٽ� �׷��ش�.
            }
        }
    }

}
