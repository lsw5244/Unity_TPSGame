using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMonster : MonoBehaviour, IMonster
{
    private float _maxHp = 100;
    [HideInInspector]
    public float currentHp;

    void Start()
    {
        currentHp = _maxHp;
    }

    private void Update()
    {

    }

    public void Die()
    {
        GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
    }

    public void GetDamage(float damage)
    {
        currentHp -= damage;
        if(currentHp <= 0f)
        {
            Die();
        }
    }

    public void PoisonEffect()
    {
        
    }
}
