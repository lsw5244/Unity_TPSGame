using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 몬스터와 충돌 x, 충돌한 물체에 IPlayer있으면 데미지, 충돌 시 오브젝트 비활성화
        if(other.gameObject.CompareTag("Monster") == false)
        {
            other.gameObject.GetComponent<IPlayer>()?.GetDamage(10f, this.gameObject);
            transform.position = Vector3.zero;
            this.gameObject.SetActive(false);
        }
    }
}
