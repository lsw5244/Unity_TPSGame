using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _bullet;
    private GameObject[] _bulletPool = new GameObject[10];

    public GameObject explosion;
    public float explosionRange = 2f;
    public float explosionDamage = 20f;

    public delegate void ImpactAbilityDelegate(GameObject bullet);
    public ImpactAbilityDelegate impactAbility;

    private void Awake()
    {
        for(int i = 0; i < 10; ++i)
        {
            _bulletPool[i] = Instantiate(_bullet, Vector3.zero, Quaternion.identity);
            _bulletPool[i].GetComponent<Bullet>().bulletManager = this;
            _bulletPool[i].SetActive(false);
        }

        //impactAbility -= BulletExplosion;
        //impactAbility += BulletExplosion;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            impactAbility(this.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            impactAbility -= BulletExplosion;
            impactAbility += BulletExplosion;
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            impactAbility -= BulletExplosion;
        }
    }

    void BulletExplosion(GameObject bullet)
    {
        Instantiate(explosion, bullet.gameObject.transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(bullet.transform.position, explosionRange, 1 << 6);
        for(int i = 0; i < colliders.Length; ++i)
        {
            colliders[i].GetComponent<IMonster>()?.GetDamage(explosionDamage);
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
