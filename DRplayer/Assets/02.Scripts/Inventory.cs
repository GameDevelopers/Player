using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    // 싱글톤 패턴 사용
    public static Inventory instance;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion

    // 아이템이 추가되면 슬롯UI에도 추가되게 만들기
    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    // 슬롯 갯수
    private int slotCnt;
    public int SlotCnt
    {
        get => slotCnt;
        set
        {
            slotCnt = value;
        }
    }

    private void Start()
    {
        SlotCnt = 16;
    }

    // 플레이어 아이템 습득 시 인벤토리로 이동
    // 획득한 아이템을 담을 리스트 생성
    public List<InvenItem> items = new List<InvenItem>();

    // 아이템을 리스트에 추가하는 메서드
    // items의 갯수가 현재 활성화된 슬롯 수보다 작을때만 아이템 추가
    public bool AddItem(InvenItem _item)
    {
        if(items.Count < SlotCnt)
        {
            items.Add(_item);
            if(onChangeItem != null)
            // 아이템 추가에 성공하면 onChangeItem를 호출
            onChangeItem.Invoke();
            return true;
        }
        // 아이템 추가에 성공하면 true 아니면 false 반환
        return false;
    }

    public void RemoveItem(int _index)
    {
        // index의 맞는 items의 속성을 제거
        items.RemoveAt(_index);
        // onChangeItem 호출해서 화면을 다시 그려준다
        onChangeItem.Invoke();
    }

    // 플레이어와 필드아이템이 충돌하면 AddItem 호출
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="FieldItem")
        {
            FieldItems fieldItems = collision.GetComponent<FieldItems>();
            // AddItem이 아이템을 추가되면 true 반환
            // 아이템 추가에 성공하면 필드아이템은 파괴
            if (AddItem(fieldItems.GetItem()))
                fieldItems.DestroyItem();
            
        }
    }
}
