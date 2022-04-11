using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // ���Ϳ� �浹 x, �浹�� ��ü�� IPlayer������ ������, �浹 �� ������Ʈ ��Ȱ��ȭ
        if(other.gameObject.CompareTag("Monster") == false)
        {
            other.gameObject.GetComponent<IPlayer>()?.GetDamage(10f, this.gameObject);
            transform.position = Vector3.zero;
            this.gameObject.SetActive(false);
        }
    }
}
