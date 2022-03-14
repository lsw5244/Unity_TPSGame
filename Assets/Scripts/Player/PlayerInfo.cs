using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour, IPlayer
{
    private float _maxHp = 100;
    [HideInInspector]
    public float currentHp;

    [SerializeField]
    private GameObject _explosionParticle;
    private bool _canHitExplosion = true;

    public float attackPower = 50f;

    public delegate void HitAbility(GameObject attacker);
    public HitAbility hitAbility;

    void Start()
    {
        currentHp = _maxHp;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            hitAbility -= HitExplosion;
            hitAbility += HitExplosion;

            GetDamage(50f, this.gameObject);
        }
    }

    public void Die()
    {
        GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
    }

    public void GetDamage(float damage, GameObject attacker)
    {
        currentHp -= damage;

        hitAbility(attacker);

        if (currentHp <= 0f)
        {
            Die();
        }
    }

    void HitExplosion(GameObject attacker)
    {
        if(_canHitExplosion == false)
        {
            return;
        }

        StartCoroutine("StartHitExplosionDelay");
        Instantiate(_explosionParticle, transform.position, Quaternion.identity);
        Collider[] coll = Physics.OverlapSphere(transform.position, 2, 1 << 6);

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

}
