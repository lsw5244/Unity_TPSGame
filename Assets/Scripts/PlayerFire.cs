using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField]
    private Transform _fireTransfrom;

    [SerializeField]
    private BulletManager _bulletManager;

    [SerializeField]
    private float _firePower;

    [SerializeField]
    private float _fireDelay;

    private bool _canFire;

    void Start()
    {
        _canFire = true;
    }

    void Update()
    {
        if(Input.GetMouseButton(0) && _canFire == true)
        {
            StartCoroutine("FireDelay");

            BulletFire();
        }
    }

    void BulletFire()
    {
        GameObject bullet = _bulletManager.GetBullet();

        if (bullet != null)
        {
            bullet.transform.position = _fireTransfrom.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);
            bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * _firePower);
        }
    }

    IEnumerator FireDelay()
    {
        _canFire = false;
        yield return new WaitForSeconds(_fireDelay);
        _canFire = true;
    }
}
