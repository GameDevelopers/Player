using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase instance;

    //���Ҽ��� ����
    public int money = 0;

    private void Awake()
    {
        instance = this;
    }
    public List<InvenItem> itemDB = new List<InvenItem>();

    // ������ ���� ������ ���� ����
    public GameObject fielditemPrefab;
    // �������� ������ ��ġ
    public Vector3[] pos;

    // ������ ���� Ȯ���� ���� ��ŸƮ �޼���
    private void Start()
    {
        // �� �ʱⰪ 10000 �Է�
        money = 10000;
        //for (int i = 0; i < 4; i++)
        //{
        //    GameObject go = Instantiate(fielditemPrefab, pos[i], Quaternion.identity);
        //    go.GetComponent<FieldItems>().SetItem(itemDB[Random.Range(0, 3)]);
        //}    
    }

}
