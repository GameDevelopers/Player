using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="ItemEft/Consumable/Health")]
public class ItemHealingEft : ItemEffect
{
    // 원하는 아이템 사용효과 구현
    public int healingPoint = 0;
    public override bool ExecuteRole()
    {
        Debug.Log("Player Add :" + healingPoint);
        return true;
    }
}
