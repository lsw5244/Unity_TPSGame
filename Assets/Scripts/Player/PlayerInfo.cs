using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour, ILivingEntity
{
    private float _maxHp = 100;
    [HideInInspector]
    public float currentHp;

    public delegate void HitAbility();
    public HitAbility hitAbility;

    void Start()
    {
        currentHp = _maxHp;
    }

    public void Die()
    {
        GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
    }

    public void GetDamage(float damage)
    {
        currentHp -= damage;
        if (currentHp <= 0f)
        {
            Die();
        }
    }
}
