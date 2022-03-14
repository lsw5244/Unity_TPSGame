using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMonster : MonoBehaviour, IMonster
{
    private float _maxHp = 100;
    [HideInInspector]
    public float currentHp;

    private bool _isPoisonState = false;
    public float poisonDamageDelay = 0.5f;
    public int poisonDamageCount = 5;

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
        poisonDamageCount = 5;

        if (_isPoisonState == false)
        {
            StartCoroutine(Poison());
        }
    }

    IEnumerator Poison()
    {
        Debug.Log($"Poison È£Ãâ !!!");

        while (poisonDamageCount > 0)
        {
            this.GetDamage(50.0f);
            Debug.Log($"@@@@@@@@@@@@@{poisonDamageCount}");

            yield return new WaitForSeconds(poisonDamageDelay);
            poisonDamageCount--;
        }


    }
}
