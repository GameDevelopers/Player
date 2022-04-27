using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ������ ������ Ÿ�� ����
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

    // ItemEffect List ����
    public List<ItemEffect> efts;

    // ������ ��� �׸� �߰�
    public int itemCost;

    public bool Use()
    {
        bool isuUsed = false;
        // �ݺ��� ������ efts�� ExecuteRole�� ����
        foreach ( ItemEffect eft in efts)
        {
            isuUsed = eft.ExecuteRole();
        }

        
        // ������ ��� ���� ���� ��ȯ
        return isuUsed;
    }
}

