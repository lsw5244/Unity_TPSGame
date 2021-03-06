using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MeleeMonster : Monster, IMonster
{
    private BoxCollider _attackCollider;

    public float attackPower = 10f;

    void Start()
    {
        currentHp = _maxHP;
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _attackCollider = GetComponent<BoxCollider>();

        _attackCollider.enabled = false;
        _playerTransform = GameObject.Find("Player").transform;/* Find("Player");*/
        StartCoroutine(StateCheck());
    }

    public void GetDamage(float damage)
    {
        if (_isAlive == false)
            return;

        Collider[] colls = Physics.OverlapSphere(transform.position, 3f, 1 << gameObject.layer);
        for(int i = 0; i < colls.Length; ++i)
        {
            colls[i].gameObject.GetComponent<IMonster>()?.StartAttentionMode();
        }

        StartAttentionMode();

        _animator.SetTrigger("Hit");
        currentHp -= damage;
        UIManager.Instance.UpdateMonsterHpbar(currentHp / _maxHP, gameObject.name);

        if (currentHp <= 0f)
        {
            Die();
        }
    }

    public override void Die()
    {
        base.Die();

        GetComponent<CapsuleCollider>().enabled = false;
        _attackCollider.enabled = false;

    }

    public void StartPoisonEffect(float damage)
    {
        poisonDamageCount = 5;

        if (_isPoisonState == false)
        {
            StartCoroutine(Poison(damage));
        }
    }

    public override void Trace()
    {
        base.Trace(); // 움직이기 + 애니메이션 변경

        // 공격이 진행중일 때 움직이지 않도록 제한 ( 계속 공격하도록 )
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("attack") == true)
        {
            // 추적 다시 중지
            _navMeshAgent.isStopped = true;
        }
        else
        {
            StopAttack();
        }
    }

    public void StartAttack()
    {
        _attackCollider.enabled = true;
    }

    public void StopAttack()
    {
        _attackCollider.enabled = false;
    }

    void OnTriggerEnter(Collider coll)
    {
        if(coll.CompareTag("Player") == true)
        {
            coll.GetComponent<IPlayer>()?.GetDamage(attackPower, this.gameObject);
            _attackCollider.enabled = false;
        }
    }

    public void StartAttentionMode()
    {
        _attentionModeContinueTrigger = true;

        if (_currentAttentionMode == false)
        {
            _currentAttentionMode = true;
            StartCoroutine(AttentionMode());
        }
    }

    //Idle, Trace, Attack, Die

}
