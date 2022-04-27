using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 아이템별로 효과를 다르게 해주기 위해 item스크립트와 별개로 작성
// ItemEffect는 추상 클래스, ScriptableObject상속받아 사용
public abstract class ItemEffect : ScriptableObject
{
    // 추상메서드
    public abstract bool ExecuteRole();
 
}
