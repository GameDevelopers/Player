using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="ItemEft/Consumable/Health")]
public class ItemHealingEft : ItemEffect
{
    // ���ϴ� ������ ���ȿ�� ����
    public int healingPoint = 0;
    public override bool ExecuteRole()
    {
        Debug.Log("Player Add :" + healingPoint);
        return true;
    }
}
