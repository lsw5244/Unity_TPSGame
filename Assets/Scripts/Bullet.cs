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
        if(other.CompareTag("Player") == false)
        {
            this.gameObject.SetActive(false);
        }
    }
}
