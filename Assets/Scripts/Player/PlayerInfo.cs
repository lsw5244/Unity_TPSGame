using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour, IPlayer
{
    private float _maxHp = 100;
    [HideInInspector]
    public float currentHp;
      
    void Start()
    {
        currentHp = _maxHp;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {      
            GetDamage(10f, this.gameObject);
        }
    }

    public void Die()
    {
        //GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
        Debug.Log("PlayerDie !!!!! In PlayerInfo");
    }

    public void GetDamage(float damage, GameObject attacker)
    {
        if(HitAbility.Instance.hitAbility != null /*&& attacker.CompareTag("Monster") == true*/)
        {
            HitAbility.Instance.hitAbility(attacker);
        }

        currentHp -= damage;

        if (currentHp <= 0f)
        {
            Die();
        }
    }
}
