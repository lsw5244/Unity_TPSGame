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
            Debug.LogWarning($"BulletAbility�� �������� �����Ǿ� {gameObject.name}�� �����մϴ�.");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            bulletAbility -= HitTarget;
            bulletAbility += HitTarget;
            Debug.Log("HitTarget Ȱ��ȭ");
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            bulletAbility -= HitTarget;
            Debug.Log("HitTarget ��Ȱ��ȭ");
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            bulletAbility -= PoisonBullet;
            bulletAbility += PoisonBullet;
            Debug.Log("PoisonBullet Ȱ��ȭ");
        }
        else if (Input.GetKeyDown(KeyCode.F6))
        {
            bulletAbility -= PoisonBullet;
            Debug.Log("PoisonBullet ��Ȱ��ȭ");
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
