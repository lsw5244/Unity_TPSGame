using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public enum State
    {
        Idle, Trace, Attack
    }
    
    [SerializeField]
    protected float _maxHP = 100f;
    [HideInInspector]
    public float currentHp;

    [SerializeField]
    protected GameObject _poisonParicle;
    protected bool _isPoisonState = false;
    public float poisonDamageDelay = 0.5f;
    public int poisonDamageCount = 5;
    
    public float traceDistance = 16f;
    public float attackDistance = 8f;

    protected State _currentState = State.Idle;

    protected bool _isAlive = true;

    protected bool _currentAttentionMode = false;
    protected bool _attentionModeContinueTrigger = false;

    protected Transform _playerTransform;
    protected Animator _animator;
    protected NavMeshAgent _navMeshAgent;

    public virtual void Idle()
    {
        // �ִϸ��̼� ����
        _animator.SetBool("Trace", false);
        // ���� ����
        _navMeshAgent.isStopped = true;
        _navMeshAgent.velocity = Vector3.zero;
    }
    public virtual void Trace()
    {
        // �ִϸ��̼� ����
        _animator.SetBool("Attack", false);
        _animator.SetBool("Trace", true);
        // ���� ����
        _navMeshAgent.isStopped = false;
        _navMeshAgent.destination = _playerTransform.position;
    }
    public virtual void Attack()
    {
        // �ִϸ��̼� ����
        _animator.SetBool("Trace", true);
        _animator.SetBool("Attack", true);

        transform.LookAt(_playerTransform.position);

        // ���� ����
        _navMeshAgent.isStopped = true;
        _navMeshAgent.velocity = Vector3.zero;
    }
    public virtual void Die()
    {
        StopAllCoroutines();

        _isAlive = false;
        _poisonParicle.SetActive(false);

        _animator.SetTrigger("Die");

        //_navMeshAgent.isStopped = true;
        //_navMeshAgent.velocity = Vector3.zero;
        _navMeshAgent.enabled = false;

        GameObject.Find("StageChanger").GetComponent<StageChanger>().RemoveMonsterCount();
    }

    public virtual void ChangeState()
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
    }

    public virtual void SelectAction()
    {
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
    }

    protected IEnumerator Poison(float damage)
    {
        _poisonParicle.SetActive(true);
        _isPoisonState = true;

        while (poisonDamageCount > 0)
        {
            if( _isAlive == false )
            {
                break;
            }

            currentHp -= damage;
            UIManager.Instance.UpdateMonsterHpbar(currentHp / _maxHP, gameObject.name);

            if (currentHp <= 0f && _isAlive == true)
            {
                Die();
                break;
            }

            yield return new WaitForSeconds(poisonDamageDelay);     // 0.5�ʿ� �� ���� ����ǵ���

            poisonDamageCount--;
        }

        _isPoisonState = false;
        _poisonParicle.SetActive(false);
    }

    protected IEnumerator AttentionMode()
    {
        traceDistance *= 2f;

        while (_attentionModeContinueTrigger == true)
        {
            _attentionModeContinueTrigger = false;

            yield return new WaitForSeconds(5f);
        }

        _currentAttentionMode = false;
        traceDistance /= 2f;
    }
}
