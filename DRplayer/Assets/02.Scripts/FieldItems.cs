using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems : MonoBehaviour
{
    // � ���������� ���
    public InvenItem item;
    // �����ۺ� �̹��� ��ȯ
    public SpriteRenderer image;

    // �ʵ忡 �������� ������ �� SetItem ���� ���޹��� item�����ͷ�
    // ���� Ŭ������ item�� �ʱ�ȭ
    public void SetItem(InvenItem _item)
    {
        // ������ �̸�, �̹���, ���� 
        item.itemName = _item.itemName;
        item.itemImage = _item.itemImage;
        item.itemType = _item.itemType;

        // ������ ����Ʈ ���� �߰�
        item.efts = _item.efts;

        // ������ ��� �Ը� �߰�
        item.itemCost = _item.itemCost;
        
        // �����ۿ� �°� �̹��� ��ȭ
        image.sprite = item.itemImage;
    }

    // �ٸ� Ŭ�������� item������ ������������ �ʰ� �Լ��� ���ؼ� �����ϱ� ���� �ۼ�
    public InvenItem GetItem()
    {
        return item;
    }
    // ������ ȹ�� �� �ʵ��� �������� �ı�
    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
