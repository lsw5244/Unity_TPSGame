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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") == false)
        {
            if (_bulletManager.impactAbility != null)
            {
                _bulletManager.impactAbility(this.gameObject);
            }

            this.gameObject.SetActive(false);
        }
    }
}
