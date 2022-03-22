using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어 이외의 오브젝트와 충돌 했을 때 처리
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
