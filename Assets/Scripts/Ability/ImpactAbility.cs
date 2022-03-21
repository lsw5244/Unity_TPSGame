using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactAbility : MonoBehaviour
{
    public static ImpactAbility _instance;
    public static ImpactAbility Instance { get { return _instance; } }

    public delegate void ImpactAbilityDelegate(GameObject bullet);
    public ImpactAbilityDelegate impactAbility;

    public GameObject explosion;
    public float explosionRange = 2f;
    public float explosionDamage = 20f;

    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.LogWarning($"ImpactAbility �������� �����Ǿ� {gameObject.name}�� �����մϴ�.");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            impactAbility -= BulletExplosion;
            impactAbility += BulletExplosion;
            Debug.Log("BulletExplosion Ȱ��ȭ");
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            impactAbility -= BulletExplosion;
            Debug.Log("BulletExplosion ��Ȱ��ȭ");
        }
    }

    /* ImactAblility */
    void BulletExplosion(GameObject bullet)
    {
        Instantiate(explosion, bullet.gameObject.transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(bullet.transform.position, explosionRange, 1 << 6);
        for (int i = 0; i < colliders.Length; ++i)
        {
            colliders[i].GetComponent<IMonster>()?.GetDamage(explosionDamage);
        }
    }
}
