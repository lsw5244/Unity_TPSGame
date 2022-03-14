using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private BulletManager _bulletManager;
    public BulletManager bulletManager
    {
        set { _bulletManager = value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") == false)
        {
            //other.gameObject.GetComponent<IMonster>()?.GetDamage(50f);
                       
            if (_bulletManager.impactAbility != null)
            {
                _bulletManager.impactAbility(this.gameObject);
            }

            if (other.gameObject.CompareTag("Monster") && _bulletManager.bulletAbilityDeleage != null)
            {
                _bulletManager.bulletAbilityDeleage(other.gameObject);
            }

            this.gameObject.SetActive(false);
        }
    }
}
