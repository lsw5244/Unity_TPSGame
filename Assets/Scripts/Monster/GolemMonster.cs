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

    [SerializeField]
    private GameObject _groundHitParicle;

    [SerializeField]
    private float _dashAttackTurnSpeed = 15f;

    private bool _runningSpacialPattern = false;

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
        if(Input.GetKeyDown(KeyCode.B))
        {
            //Instantiate(_groundHitParicle, transform.position, Quaternion.identity);
            //StartDashAttack();
            _animator.SetTrigger("DashAttack");
        }

    }
    
    void StartDashAttack()
    {
        Debug.Log("Start!!");
        _runningSpacialPattern = true;
        _navMeshAgent.enabled = false;
        transform.LookAt(_playerTransform);
    }

    void EndDashAttack()
    {
        Debug.Log("End!!");
        _runningSpacialPattern = false;
        _navMeshAgent.enabled = true;
    }

    void DashAttack()
    {
        Vector3 playerDistance = _playerTransform.position - transform.position;

        // 앞에 있는지 확인
        if(Vector3.Dot(transform.forward, playerDistance) < 0f)
        {
            return;
        }
        // 좌, 우 어느쪽에 있는지 확인
        if(Vector3.Cross(transform.forward, playerDistance).y < 0f) // 왼쪽에 있을 때
        {
            transform.Rotate(0f, -_dashAttackTurnSpeed, 0f);
        }
        else  // 오른쪽에 있을 때
        {
            transform.Rotate(0f, _dashAttackTurnSpeed, 0f);
        }
    }

    IEnumerator TraceCheck()
    {
        while (_isAlive == true)
        {
            if(_runningSpacialPattern == true)
            {
                DashAttack();
            }

            ChangeState();
            
            if(_runningSpacialPattern == false)
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

            yield return new WaitForSeconds(0.3f);
        }
    }

    void ChangeState()
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

    public override void Attack()
    {
        // 애니메이션 변경
        _animator.SetBool("Attack", true);

        transform.LookAt(_playerTransform.position);

        // 추적 중지
        _navMeshAgent.isStopped = true;
        _navMeshAgent.velocity = Vector3.zero;
    }

    public void StartAttack()
    {
        _attackCollider.enabled = true;
    }

    public void StopAttack()
    {
        _attackCollider.enabled = false;
    }

    public override void Die()
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

//      _animator.SetTrigger("Hit");
        currentHp -= damage;
        if (currentHp <= 0f)
        {
            Die();
        }
        //Debug.Log($"Golem HP : {currentHp}");
    }

    IEnumerator AttentionMode()
    {
        traceDistance *= 2f;

        while (_continueAttentionMode == true)
        {
            _continueAttentionMode = false;

            yield return new WaitForSeconds(5f);
        }

        _attentionModeTrigger = false;
        traceDistance /= 2f;
    }

    public override void Idle()
    {
        // 애니메이션 변경
        _animator.SetBool("Trace", false);
        // 추적 중지
        _navMeshAgent.isStopped = true;
        _navMeshAgent.velocity = Vector3.zero;
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

    public override void Trace()
    {
        // 애니메이션 변경
        _animator.SetBool("Attack", false);
        StopAttack();
        _animator.SetBool("Trace", true);

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("attack") == false)
        {
            // 추적 시작
            _navMeshAgent.isStopped = false;
            _navMeshAgent.destination = _playerTransform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            other.GetComponent<IPlayer>()?.GetDamage(attackPower, this.gameObject);
            _attackCollider.enabled = false;
        }
    }
}
