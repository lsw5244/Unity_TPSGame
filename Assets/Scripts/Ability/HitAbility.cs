using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAbility : MonoBehaviour
{
    public static HitAbility _instance;
    public static HitAbility Instance { get { return _instance; } }

    public delegate void HitAbilityDelegate(GameObject attacker);
    public HitAbilityDelegate hitAbility;

    [SerializeField]
    private GameObject _explosionParticle;
    private bool _canHitExplosion = true;

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
            Debug.LogWarning($"HitAbility 여러개가 감지되어 {gameObject.name}를 삭제합니다.");
            Destroy(gameObject);
        }
    }
    public void ClearAbility()
    {
        hitAbility = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hitAbility -= HitPoisonDamage;
            hitAbility += HitPoisonDamage;
        }
    }

    /* hitExplosion */
    void HitExplosion(GameObject attacker)
    {
        if (_canHitExplosion == false)
        {
            return;
        }

        StartCoroutine(StartHitExplosionDelay());
        Instantiate(_explosionParticle, attacker.transform.position, Quaternion.identity);
        Collider[] coll = Physics.OverlapSphere(attacker.transform.position, 2, 1 << 6);

        for (int i = 0; i < coll.Length; ++i)
        {
            coll[i].GetComponent<IMonster>()?.GetDamage(30f);
        }
    }

    IEnumerator StartHitExplosionDelay()
    {
        _canHitExplosion = false;
        yield return new WaitForSeconds(1f);
        _canHitExplosion = true;
    }

    public void AddHitExplosion()
    {
        hitAbility -= HitExplosion;
        hitAbility += HitExplosion;
    }

    void HitPoisonDamage(GameObject attacker)
    {
        attacker.GetComponent<IMonster>()?.PoisonEffect(poisonDamage);
    }

    public void AddHitPoisonDamage()
    {
        hitAbility -= HitPoisonDamage;
        hitAbility += HitPoisonDamage;
    }
}
