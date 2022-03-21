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
            Debug.LogWarning($"ImpactAbility 여러개가 감지되어 {gameObject.name}를 삭제합니다.");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            impactAbility -= BulletExplosion;
            impactAbility += BulletExplosion;
            Debug.Log("BulletExplosion 활성화");
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            impactAbility -= BulletExplosion;
            Debug.Log("BulletExplosion 비활성화");
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
