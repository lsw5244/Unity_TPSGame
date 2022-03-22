using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾� �̿��� ������Ʈ�� �浹 ���� �� ó��
        if (other.gameObject.CompareTag("Player") == false)
        {                     
            if (ImpactAbility.Instance.impactAbility != null)
            {
                ImpactAbility.Instance.impactAbility(this.gameObject);
            }
            
            if (other.gameObject.CompareTag("Monster") && BulletAbility.Instance.bulletAbility != null)
            {
                BulletAbility.Instance.bulletAbility(other.gameObject);
            }

            this.gameObject.SetActive(false);
        }
    }
}
