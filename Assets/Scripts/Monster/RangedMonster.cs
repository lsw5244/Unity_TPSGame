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

    private State _currentState = State.Idle;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        currentHp = _maxHP;

        _playerTransform = GameObject.Find("Player").transform;/* Find("Player");*/
    }

    void Update()
    {
        
    }


    public void Attack()
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
        throw new System.NotImplementedException();
    }

    public void PoisonEffect(float damage)
    {
        throw new System.NotImplementedException();
    }

    public void Trace()
    {
        throw new System.NotImplementedException();
    }
}
