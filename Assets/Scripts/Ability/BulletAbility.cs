using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAbility : MonoBehaviour
{
    public static BulletAbility _instance;
    public static BulletAbility Instance { get { return _instance; } }

    public delegate void BulletAbilityDeleage(GameObject target);
    public BulletAbilityDeleage bulletAbility;

    public float bulletDamage = 25f;
    public float poisonDamage = 20f;

    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.LogWarning($"BulletAbility가 여러개가 감지되어 {gameObject.name}를 삭제합니다.");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            bulletAbility -= HitTarget;
            bulletAbility += HitTarget;
            Debug.Log("HitTarget 활성화");
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            bulletAbility -= HitTarget;
            Debug.Log("HitTarget 비활성화");
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            bulletAbility -= PoisonBullet;
            bulletAbility += PoisonBullet;
            Debug.Log("PoisonBullet 활성화");
        }
        else if (Input.GetKeyDown(KeyCode.F6))
        {
            bulletAbility -= PoisonBullet;
            Debug.Log("PoisonBullet 비활성화");
        }
    }

    /* BulletAbility */
    void HitTarget(GameObject target)
    {
        target.GetComponent<IMonster>().GetDamage(bulletDamage);
    }

    void PoisonBullet(GameObject target)
    {
        target.GetComponent<IMonster>().PoisonEffect(poisonDamage);
    }
}
