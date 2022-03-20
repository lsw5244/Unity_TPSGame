using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MeleeMonster : MonoBehaviour, IMonster
{
    enum State
    {
        Idle, Trace, Attack, Die
    }
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;

    [SerializeField]
    private float _maxHP = 100f;
    [HideInInspector]
    public float currentHp;

    private bool _isPoisonState = false;
    public float poisonDamageDelay = 0.5f;
    public int poisonDamageCount = 5;

    [SerializeField]
    private GameObject _poisonParicle;

    private Transform _playerTransform;

    private bool _isAlive = true;

    public float traceDistance = 5f;
    public float attackDistance = 3f;

    private State _currentState = State.Idle;

    void Start()
    {
        currentHp = _maxHP;
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _playerTransform = GameObject.Find("Player").transform;/* Find("Player");*/
        StartCoroutine(TraceCheck());
    }

    void Update()
    {

    }

    IEnumerator TraceCheck()
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
                //_navMeshAgent.updatePosition = false;
                Debug.Log("@@@@");

                _navMeshAgent.velocity = Vector3.zero;
            }

            yield return new WaitForSeconds(0.3f);
        }

    }

    public void GetDamage(float damage)
    {
        if (_isAlive == false)
            return;

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
        _isAlive = false;

        _animator.SetTrigger("Die");
        
        _navMeshAgent.isStopped = true;
        _navMeshAgent.velocity = Vector3.zero;

        GetComponent<CapsuleCollider>().enabled = false;
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
            if (currentHp <= 0f)
            {
                Die();
            }

            yield return new WaitForSeconds(poisonDamageDelay);     // 0.5초에 한 번씩 실행되도록

            poisonDamageCount--;
        }

        _isPoisonState = false;
        _poisonParicle.SetActive(false);
    }

    public void Attack()
    {
        // 애니메이션 변경
        _animator.SetBool("Attack", true);

        transform.LookAt(_playerTransform.position);

        // 추적 중지
        _navMeshAgent.isStopped = true;
        _navMeshAgent.velocity = Vector3.zero;
    }

    void Idle()
    {
        // 애니메이션 변경
        _animator.SetBool("Trace", false);
        // 추적 중지
        _navMeshAgent.isStopped = true;
        _navMeshAgent.velocity = Vector3.zero;
    }

    void Trace()
    {
        // 애니메이션 변경
        _animator.SetBool("Attack", false);
        _animator.SetBool("Trace", true);
        // 추적 시작
        _navMeshAgent.isStopped = false;
        _navMeshAgent.destination = _playerTransform.position;
    }

    //Idle, Trace, Attack, Die
}
