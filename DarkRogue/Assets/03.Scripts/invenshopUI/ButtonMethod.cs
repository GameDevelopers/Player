using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonMethod : MonoBehaviour
{
    public Button button;

    // ��ư ����
    public void Enter(Button button)
    {
        button.Select();
    }

    void Start()
    {
        button.Select();
    }

    
    
}
