using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems : MonoBehaviour
{
    // 어떤 아이템인지 명시
    public InvenItem item;
    // 아이템별 이미지 변환
    public SpriteRenderer image;

    // 필드에 아이템을 생성할 때 SetItem 통해 전달받은 item데이터로
    // 현재 클래스의 item을 초기화
    public void SetItem(InvenItem _item)
    {
        // 아이템 이름, 이미지, 종류 
        item.itemName = _item.itemName;
        item.itemImage = _item.itemImage;
        item.itemType = _item.itemType;

        // 아이템 이펙트 변수 추가
        item.efts = _item.efts;

        // 아이템 비용 함목 추가
        item.itemCost = _item.itemCost;
        
        // 아이템에 맞게 이미지 변화
        image.sprite = item.itemImage;
    }

    // 다른 클래스에서 item변수를 직접가져오지 않고 함수를 통해서 접근하기 위해 작성
    public InvenItem GetItem()
    {
        return item;
    }
    // 아이템 획득 시 필드의 아이템은 파괴
    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
