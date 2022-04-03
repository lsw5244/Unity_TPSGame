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

    void Update()
    {

    }

    IEnumerator StateCheck()
    {
        while(_isAlive == true)
        {
            float distance = Vector3.Distance(transform.position, _playerTransform.position);

            if (distance < attackDistance)
            {
                _currentState = State.Attack;
            }
            else if (distance < traceDistance)
            {
                _currentState = State.Trace;
            }
            else
            {
                _currentState = State.Idle;
            }

            switch (_currentState)
            {
                case State.Attack:
                    Attack();
                    break;
                case State.Trace:
                    Trace();
                    break;
                case State.Idle:
                    Idle();
                    break;
            }

            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("hit") == true)
            {
                _navMeshAgent.velocity = Vector3.zero;
            }

            yield return new WaitForSeconds(0.3f);
        }
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

    IEnumerator AttentionMode()
    {
        traceDistance *= 2f;

        while(_continueAttentionMode == true)
        {
            _continueAttentionMode = false;

            yield return new WaitForSeconds(5f);
        }

        _attentionModeTrigger = false;
        traceDistance /= 2f;
    }

    public override void Die()
    {
        base.Die();

        GetComponent<CapsuleCollider>().enabled = false;
        _attackCollider.enabled = false;

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
            currentHp -= damage;
            UIManager.Instance.UpdateMonsterHpbar(currentHp / _maxHP, gameObject.name);
            if (currentHp <= 0f && _isAlive == true)
            {
                Die();
                break;
            }

            yield return new WaitForSeconds(poisonDamageDelay);     // 0.5초에 한 번씩 실행되도록

            poisonDamageCount--;
        }

        _isPoisonState = false;
        _poisonParicle.SetActive(false);
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
        _continueAttentionMode = true;

        if (_attentionModeTrigger == false)
        {
            _attentionModeTrigger = true;
            StartCoroutine(AttentionMode());
        }
    }

    //Idle, Trace, Attack, Die

}
