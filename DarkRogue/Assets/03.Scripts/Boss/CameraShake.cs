using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ī�޶� ��鸲�� �����ϴ� ��ũ��Ʈ
public class CameraShake : MonoBehaviour
{
    // ��鸲 ����
    public float ShakeAmount;
    // ��鸲 �ð�
    private float ShakeTime;
    // ���� ī�޶� ��ġ�ϴ� ����
    private Vector3 initialPos;

    // ���� �ð� �޼���
    public void VibrateFormTime(float time)
    {
        // ��鸱 �ð��� time������ �ʱ�ȭ
        ShakeTime = time;
    }

    // ���� ī�޶� ��ġ�� ���͸� �ʱ�ȭ
    void Start()
    {
        // (8,1,-5)�� ��ġ�� ����
        initialPos = new Vector3(8f, 1f, -5f);
    }

    void Update()
    {
        // ���� ��鸲 �ð��� 0���� ũ��
        if (ShakeTime > 0)
        {
            // �ݰ�(ShakeAmount + initialPos) ���� ���� ��ġ ����
            transform.position = Random.insideUnitSphere * ShakeAmount + initialPos;
            ShakeTime -= Time.deltaTime;
        }
        // �׿�
        else
        {
            // ��鸲 �ð� = 0
            ShakeTime = 0.0f;
            // ���� ��ġ�� �ǵ���
            transform.position = initialPos;
        }
    }
}
