using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ShopData : MonoBehaviour
{
    // 게임이 시작되면 리스트에 아이템이 추가됨
    public List<InvenItem> stocks = new List<InvenItem>();
    // 아이템이 팔리면 리스트와 같은 위치를 참으로 만든다
    public bool[] soldOuts;

    void Start()
    {
        //// 테스트를 위한 아이템 추가
        //stocks.Add(ItemDataBase.instance.itemDB[0]);
      
        // 해당아이템이 팔린 위치 정보를 가지고 있을 것
        soldOuts = new bool[stocks.Count];
        // 배열을 리스트의 크기만큼 초기화하고 반복문을 통해 배열을 전부 거짓으로 초기화
        for (int i = 0; i < soldOuts.Length; i++)
        {
            soldOuts[i] = false;
        }
    }
}
