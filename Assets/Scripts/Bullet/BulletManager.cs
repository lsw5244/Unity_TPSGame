using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _bullet;
    private GameObject[] _bulletPool = new GameObject[10];

    [SerializeField]
    private GameObject _fireBall;
    private GameObject[] _fireBallPool = new GameObject[10];

    [SerializeField]
    private PlayerInfo _playerInfo;

    private void Awake()
    {
        for(int i = 0; i < 10; ++i)
        {
            _bulletPool[i] = Instantiate(_bullet, Vector3.zero, Quaternion.identity);
            _fireBallPool[i] = Instantiate(_fireBall, Vector3.zero, Quaternion.identity);

            _bulletPool[i].SetActive(false);
            _fireBallPool[i].SetActive(false);
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

    public GameObject GetFireBall()
    {
        for (int i = 0; i < 10; ++i)
        {
            if (_fireBallPool[i].activeSelf == false)
            {
                return _fireBallPool[i];
            }
        }

        return null;
    }
  
}
