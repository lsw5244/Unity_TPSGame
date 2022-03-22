using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MeleeMonster : MonoBehaviour, IMonster
{
    enum State
    {
        Idle, Trace, Attack
    }
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private BoxCollider _attackCollider;

    [SerializeField]
    private float _maxHP = 100f;
    [HideInInspector]
    public float currentHp;
    public float attackPower = 10f;

    private bool _isPoisonState = false;
    public float poisonDamageDelay = 0.5f;
    public int poisonDamageCount = 5;

    [SerializeField]
    private GameObject _poisonParicle;

    private Transform _playerTransform;

    private bool _isAlive = true;
    private bool _attentionModeTrigger = false;
    private bool _continueAttentionMode = false;

    public float traceDistance = 8f;
    public float attackDistance = 2f;

    private State _currentState = State.Idle;

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
                _navMeshAgent.velocity = Vector3.zero;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    public void GetDamage(float damage)
    {
        if (_isAlive == false)
            return;

        _continueAttentionMode = true;

        if (_attentionModeTrigger == false)
        {
            _attentionModeTrigger = true;
            StartCoroutine(AttentionMode());
        }

        _animator.SetTrigger("Hit");
        currentHp -= damage;
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

    public void Die()
    {
        _isAlive = false;

        StopAllCoroutines();
        _poisonParicle.SetActive(false);

        _animator.SetTrigger("Die");
        
        _navMeshAgent.isStopped = true;
        _navMeshAgent.velocity = Vector3.zero;

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

    public void StartAttack()
    {
        _attackCollider.enabled = true;
    }

    public void StopAttack()
    {
        _attackCollider.enabled = false;
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

    public void Idle()
    {
        // 애니메이션 변경
        _animator.SetBool("Trace", false);
        // 추적 중지
        _navMeshAgent.isStopped = true;
        _navMeshAgent.velocity = Vector3.zero;
    }

    public void Trace()
    {
        // 애니메이션 변경
        _animator.SetBool("Attack", false);
        _animator.SetBool("Trace", true);
        // 추적 시작
        _navMeshAgent.isStopped = false;
        _navMeshAgent.destination = _playerTransform.position;
    }

    void OnTriggerEnter(Collider coll)
    {
        if(coll.CompareTag("Player") == true)
        {
            coll.GetComponent<IPlayer>()?.GetDamage(attackPower, this.gameObject);
            _attackCollider.enabled = false;
        }
    }

    //Idle, Trace, Attack, Die

}
