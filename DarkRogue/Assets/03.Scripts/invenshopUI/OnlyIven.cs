using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OnlyIven : MonoBehaviour
{
    // �κ��丮 ���� ����
    Inventory inven;

    // �κ��丮 �гο� Inventory UI Panel ������Ʈ�� ����ش�.
    public GameObject inventoryPanel;

    // Ȱ��ȭ ���� �Ǵ�
    bool activeInventory = false;

    // �����վ��� ���Ե��� ������
    public Slot[] slots;
    public Transform slotHolder;

    // �Ͻ����� ����
    public bool isPause = false;


    private void Start()
    {
        // �κ��丮 ���� �ʱ�ȭ
        inven = Inventory.instance;

        // GetComponentsInChildren�̿��ؼ� content���� Slot �����Ǵ°� ���� ����
        slots = slotHolder.GetComponentsInChildren<Slot>();

        //onChangeItem�� ������ �޼��� ����
        inven.onChangeItem += RedrawSlotUI;

        // �ѹ� ȣ���ؼ� ��� ������ �ʱ�ȭ
        RedrawSlotUI();

        // �ʱ⿡ �κ��丮 ������ ���·� ����
        inventoryPanel.SetActive(activeInventory);
    }

    private void RedrawSlotUI()
    {
        // �ݺ����� ���� ���Ե��� �ʱ�ȭ
        // items�� ������ŭ slot�� ä���ֽ��ϴ�.
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
    private void Update()
    {
        // I Ű�� �κ��丮 â Ȱ��ȭ + ������ �������� �ʴٸ�
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!isPause)
            {
                SoundManager.sm.UIonoffPlay();
                // �Ͻ����� ���� �ƴϸ� �Ͻ�����
                Time.timeScale = 0; // �ð�����
                activeInventory = !activeInventory;
                inventoryPanel.SetActive(activeInventory); // �г�Ȱ��ȭ
            }
            else
            {
                SoundManager.sm.UIonoffPlay();
                Time.timeScale = 1.0f; // �ð��帧 ���� 1
                activeInventory = !activeInventory;
                inventoryPanel.SetActive(activeInventory);
            }

            isPause = !isPause; // ���� ������ ���°� �ٲ�
        }
    }
}
