using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    // �̱��� ���� ���
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

    // �������� �߰��Ǹ� ����UI���� �߰��ǰ� �����
    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    // ���� ����
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

    // �÷��̾� ������ ���� �� �κ��丮�� �̵�
    // ȹ���� �������� ���� ����Ʈ ����
    public List<InvenItem> items = new List<InvenItem>();

    // �������� ����Ʈ�� �߰��ϴ� �޼���
    // items�� ������ ���� Ȱ��ȭ�� ���� ������ �������� ������ �߰�
    public bool AddItem(InvenItem _item)
    {
        if(items.Count < SlotCnt)
        {
            items.Add(_item);
            if(onChangeItem != null)
            // ������ �߰��� �����ϸ� onChangeItem�� ȣ��
            onChangeItem.Invoke();
            return true;
        }
        // ������ �߰��� �����ϸ� true �ƴϸ� false ��ȯ
        return false;
    }

    public void RemoveItem(int _index)
    {
        // index�� �´� items�� �Ӽ��� ����
        items.RemoveAt(_index);
        // onChangeItem ȣ���ؼ� ȭ���� �ٽ� �׷��ش�
        onChangeItem.Invoke();
    }

    // �÷��̾�� �ʵ�������� �浹�ϸ� AddItem ȣ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="FieldItem")
        {
            FieldItems fieldItems = collision.GetComponent<FieldItems>();
            // AddItem�� �������� �߰��Ǹ� true ��ȯ
            // ������ �߰��� �����ϸ� �ʵ�������� �ı�
            if (AddItem(fieldItems.GetItem()))
                fieldItems.DestroyItem();
            
        }
    }
}
