using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AtkType { Circle }

public class BossWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject bossBulletPrefab;

    public void StartFire(AtkType atkType)
    {
        StartCoroutine(atkType.ToString());
    }

    public void StopFire(AtkType atkType)
    {
        StopCoroutine(atkType.ToString());
    }

    private IEnumerator Circle()
    {
        // ��ǥ = �߾�
        //Vector3 Pos = new Vector3(0, -2, 0);
        //float atkRate = 2f;
        float atkRate = 1.0f;
        int count = 6;
        float iAngle = 360 / count;
        float wAngle = 0;

        //while (true)
        //{
        //    // �߻�ü ����
        //    GameObject clone = Instantiate(bossBulletPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        //    GameObject clone1 = Instantiate(bossBulletPrefab, transform.position + Vector3.down * 0.9f, Quaternion.identity);
        //    // �߻�ü �̵�����
        //    Vector3 direction = (Pos - clone.transform.position).normalized;
        //    // �߻�ü �̵� ���� ����
        //    clone.GetComponent<Movement>().Move(direction);
        //    clone1.GetComponent<Movement>().Move(direction);

        //    // atkRate�ð� ��ŭ ���
        //    yield return new WaitForSeconds(atkRate);
        //}
        while (true)
        {
            for (int i = 0; i < count; ++i)
            {
                GameObject clone = Instantiate(bossBulletPrefab, transform.position, Quaternion.identity);
                // �߻�ü �̵� ����(����)
                float angle = wAngle + iAngle * (i) * (-0.1f);
                // �߻�ü �̵� ����(����)
                float x = Mathf.Cos(angle * Mathf.PI / 10.0f);
                float y = Mathf.Sin(angle * Mathf.PI / 10.0f);
                // �߻�ü �̵����� ����
                clone.GetComponent<Movement>().Move(new Vector2(x, y));
            }
            // �߻�ü�� �����Ǵ� ���� ���� ���� ����
            wAngle += 1;

            // ���� �ֱ⸸ŭ ���
            yield return new WaitForSeconds(atkRate);
        }
    }
}
