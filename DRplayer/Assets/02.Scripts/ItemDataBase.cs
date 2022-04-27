using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase instance;

    //지불수단 변수
    public int money = 0;

    private void Awake()
    {
        instance = this;
    }
    public List<InvenItem> itemDB = new List<InvenItem>();

    // 프리팹 복제 생성을 위한 변수
    public GameObject fielditemPrefab;
    // 아이템을 생성할 위치
    public Vector3[] pos;

    // 아이템 습득 확인을 위한 스타트 메서드
    private void Start()
    {
        // 돈 초기값 10000 입력
        money = 10000;
        //for (int i = 0; i < 4; i++)
        //{
        //    GameObject go = Instantiate(fielditemPrefab, pos[i], Quaternion.identity);
        //    go.GetComponent<FieldItems>().SetItem(itemDB[Random.Range(0, 3)]);
        //}    
    }

}
