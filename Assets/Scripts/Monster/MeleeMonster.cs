using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMonster : MonoBehaviour, IMonster
{
    enum State
    {
        Idle, Trace, Attack, Hit, Die
    }
    private Animator _animator;

    [SerializeField]
    private float _maxHP = 100f;
    [HideInInspector]
    public float currentHp;

    private bool _isPoisonState = false;
    public float poisonDamageDelay = 0.5f;
    public int poisonDamageCount = 5;

    [SerializeField]
    private GameObject _poisonParicle;

    void Start()
    {
        currentHp = _maxHP;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            _animator.SetBool("Trace", true);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            GetDamage(1f);
        }
    }

    public void GetDamage(float damage)
    {
        _animator.SetTrigger("Hit");
        currentHp -= damage;
        if (currentHp <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("MeleeMonster Die!!!!");
    }

    public void PoisonEffect(float damage)
    {
        poisonDamageCount = 5;

        if (_isPoisonState == false)
        {
            StartCoroutine(Poison(damage));
        }
    }

    IEnumerator Poison(float damage)
    {
        _poisonParicle.SetActive(true);
        _isPoisonState = true;

        while (poisonDamageCount > 0)
        {
            this.GetDamage(damage);

            yield return new WaitForSeconds(poisonDamageDelay);     // 0.5초에 한 번씩 실행되도록

            poisonDamageCount--;
        }

        _isPoisonState = false;
        _poisonParicle.SetActive(false);
    }

    public void Attack()
    {
        Debug.Log("Attack");
    }
}
