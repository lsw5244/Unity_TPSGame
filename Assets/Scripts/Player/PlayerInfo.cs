using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour, IPlayer
{
    private float _maxHp = 100;
    [HideInInspector]
    public float currentHp;

    public delegate void HitAbility(GameObject attacker);
    public HitAbility hitAbility;

    [SerializeField]
    private GameObject _explosionParticle;

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
        Debug.Log($"Attacer : {attacker.tag}, HitExplosionCall!!!!!!!");
        Instantiate(_explosionParticle, transform.position, Quaternion.identity);
    }

}
