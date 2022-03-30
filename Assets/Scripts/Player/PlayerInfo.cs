using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour, IPlayer
{
    [SerializeField]
    private GameObject _gun;

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
            //GetDamage(10f, this.gameObject);
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("PlayerDie !!!!! In PlayerInfo");
        GetComponent<PlayerAnimation>().PlayerDie();
        _gun.GetComponent<Rigidbody>().useGravity = true;
        _gun.GetComponent<BoxCollider>().enabled = true;
        GetComponent<PlayerCamera>().enabled = false;
        GetComponent<PlayerMove>().enabled = false;
        GetComponent<PlayerRotate>().enabled = false;
        GetComponent<PlayerFire>().enabled = false;
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

        Debug.Log($"Player GetDamage !!! CurrentHp : {currentHp}");
    }
}
