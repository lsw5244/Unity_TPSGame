using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _bullet;

    private GameObject[] _bulletPool = new GameObject[10];
    private void Awake()
    {
        for(int i = 0; i < 10; ++i)
        {
            _bulletPool[i] = Instantiate(_bullet, Vector3.zero, Quaternion.identity);
            _bulletPool[i].SetActive(false);
        }
    }

    public GameObject GetBullet()
    {
        for(int i = 0; i < 10; ++i)
        {
            if(_bulletPool[i].activeSelf == false)
            {
                return _bulletPool[i];
            }
        }

        return null;
    }
}
