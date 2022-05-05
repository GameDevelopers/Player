using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpView : MonoBehaviour
{
    [SerializeField]
    private Boss boss;
    private Slider Hp;

    private void Awake()
    {
        Hp = GetComponent<Slider>();

    }
    void Update()
    {
        Hp.value = boss.CurrentHP / boss.MaxHP;
    }
}
