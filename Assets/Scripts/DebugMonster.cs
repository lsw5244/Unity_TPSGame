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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Die();
        }
    }

    public void Die()
    {
        GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
    }

    public void GetDamage(float damage)
    {
        Debug.Log("GETDAMAGE!!!");
        currentHp -= damage;
        if(currentHp <= 0f)
        {
            Die();
        }
    }
}
