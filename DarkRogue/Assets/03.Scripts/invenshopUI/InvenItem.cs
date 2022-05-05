using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 아이템 생성될 타입 종류
public enum InvenItemType
{
    Equipment,
    Consumables,
    Etc
}

[System.Serializable]
public class InvenItem 
{
    public InvenItemType itemType;
    public string itemName;
    public Sprite itemImage;

    // ItemEffect List 생성
    public List<ItemEffect> efts;

    // 아이템 비용 항목 추가
    public int itemCost;

    public bool Use()
    {
        bool isuUsed = false;
        // 반복문 돌려서 efts의 ExecuteRole을 실행
        foreach ( ItemEffect eft in efts)
        {
            isuUsed = eft.ExecuteRole();
        }

        
        // 아이템 사용 성공 여부 반환
        return isuUsed;
    }
}

