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

    protected bool _attentionModeTrigger = false;
    protected bool _continueAttentionMode = false;

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

    }
    public virtual void Die()
    {

    }
}
