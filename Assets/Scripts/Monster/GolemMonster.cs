using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GolemMonster : Monster, IMonster
{
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
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
        StartCoroutine(TraceCheck());
    }

    IEnumerator TraceCheck()
    {
        while (_isAlive == true)
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

            //if (_animator.GetCurrentAnimatorStateInfo(0).IsName("hit") == true)
            //{
            //    _navMeshAgent.velocity = Vector3.zero;
            //}

            yield return new WaitForSeconds(0.3f);
        }
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    public void GetDamage(float damage)
    {
        throw new System.NotImplementedException();
    }

    public override void Idle()
    {
        throw new System.NotImplementedException();
    }

    public void PoisonEffect(float damage)
    {
        throw new System.NotImplementedException();
    }

    public override void Trace()
    {
        throw new System.NotImplementedException();
    }
}
