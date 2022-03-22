using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RangedMonster : MonoBehaviour, IMonster
{
    enum State
    {
        Idle, Trace, Attack
    }
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;

    [SerializeField]
    private float _maxHP = 100f;
    [HideInInspector]
    public float currentHp;
    public float attackPower = 10f;

    [SerializeField]
    private GameObject _poisonParicle;
    private bool _isPoisonState = false;
    public float poisonDamageDelay = 0.5f;
    public int poisonDamageCount = 5;

    private Transform _playerTransform;

    private bool _isAlive = true;

    private bool _attentionModeTrigger = false;
    private bool _continueAttentionMode = false;

    public float traceDistance = 16f;
    public float attackDistance = 8f;

    private State _currentState = State.Idle;

    [SerializeField]
    private GameObject _leftHandFireParticle;
    [SerializeField]
    private GameObject _rightHandFireParticle;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        currentHp = _maxHP;

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

            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("hit") == true)
            {
                _navMeshAgent.velocity = Vector3.zero;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    public void PoisonEffect(float damage)
    {
        throw new System.NotImplementedException();
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }

    public void GetDamage(float damage)
    {
        throw new System.NotImplementedException();
    }

    public void Idle()
    {
        // �ִϸ��̼� ����
        _animator.SetBool("Trace", false);
        // ���� ����
        _navMeshAgent.isStopped = true;
        _navMeshAgent.velocity = Vector3.zero;
    }

    public void Attack()
    {
        // �ִϸ��̼� ����
        _animator.SetBool("Trace", true);
        _animator.SetBool("Attack", true);

        transform.LookAt(_playerTransform.position);

        // ���� ����
        _navMeshAgent.isStopped = true;
        _navMeshAgent.velocity = Vector3.zero;
    }

    public void Trace()
    {
        // �ִϸ��̼� ����
        _animator.SetBool("Attack", false);
        _animator.SetBool("Trace", true);
        // ���� ����
        _navMeshAgent.isStopped = false;
        _navMeshAgent.destination = _playerTransform.position;
    }
}
