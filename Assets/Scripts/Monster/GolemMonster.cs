using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GolemMonster : Monster, IMonster
{
    private BoxCollider _attackCollider;

    public float attackPower = 10f;

    [SerializeField]
    private GameObject _groundHitParicle;
    [SerializeField]
    private Transform _grountAttackTransform;

    [SerializeField]
    private float _dashAttackTurnSpeed = 15f;

    private bool _runningDashAttack = false;
    private bool _runningGroundAttack = false;

    [SerializeField]
    private GameObject _fragments;
    [SerializeField]
    private float _groundAttackRange = 5f;

    void Start()
    {
        currentHp = _maxHP;
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _attackCollider = GetComponent<BoxCollider>();

        _attackCollider.enabled = false;

        _playerTransform = GameObject.Find("Player").transform;/* Find("Player");*/
        StartCoroutine(StateCheck());
        StartCoroutine(SpacialAttackCheck());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            if (Random.Range(0.0f, 2.0f) > 1f)
            {
                _animator.SetTrigger("DashAttack");
            }
            else
            {
                _animator.SetTrigger("GroundAttack");
            }
        }
    }
    
    void StartGroundAttack()
    {
        _runningGroundAttack = true;
        _navMeshAgent.enabled = false;
    }

    void EndGroundAttack()
    {
        _runningGroundAttack = false;
        _navMeshAgent.enabled = true;
    }

    void CreateFragments()
    {
        if(Vector3.Distance(_playerTransform.position, _grountAttackTransform.position) < _groundAttackRange)
        {
            _playerTransform.gameObject.GetComponent<IPlayer>()?.GetDamage(attackPower, this.gameObject);
        }

        Instantiate(_groundHitParicle, _grountAttackTransform.position, Quaternion.identity);
        Instantiate(_fragments, _grountAttackTransform.position, Quaternion.identity);
    }

    IEnumerator SpacialAttackCheck()
    {
        while(true)
        {
            yield return new WaitForSeconds(10f);

            if (_currentState == State.Trace)
            {
                if ( Random.Range(0.0f, 2.0f) > 1f )
                {
                    _animator.SetTrigger("DashAttack");
                }
                else
                {
                    _animator.SetTrigger("GroundAttack");
                }
            }
        }        
    }

    void StartDashAttack()
    {
        _runningDashAttack = true;
        _navMeshAgent.enabled = false;

        _attackCollider.enabled = true;

        transform.LookAt(_playerTransform);
    }

    void EndDashAttack()
    {
        _runningDashAttack = false;
        _navMeshAgent.enabled = true;

        _attackCollider.enabled = true;
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

    protected override IEnumerator StateCheck()
    {
        while (_isAlive == true)
        {
            yield return new WaitForSeconds(0.3f);

            if(_runningDashAttack == true)
            {
                DashAttack();
                continue;
            }

            if(_runningGroundAttack == true)
            {
                continue;
            }

            ChangeState();
            SelectAction();
        }
    }

    //public override void SelectAction()
    //{
    //    if (_runningDashAttack == false)
    //    {
    //        base.SelectAction();
    //    }
    //}

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
        base.Die();

        GetComponent<CapsuleCollider>().enabled = false;
        _attackCollider.enabled = false;
    }

    public void GetDamage(float damage)
    {
        if (_isAlive == false)
            return;

        Collider[] colls = Physics.OverlapSphere(transform.position, 3f, 1 << gameObject.layer);
        for (int i = 0; i < colls.Length; ++i)
        {
            colls[i].gameObject.GetComponent<IMonster>()?.StartAttentionMode();
        }

        StartAttentionMode();

        currentHp -= damage;
        UIManager.Instance.UpdateMonsterHpbar(currentHp / _maxHP, gameObject.name);
        if (currentHp <= 0f)
        {
            Die();
        }
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
        base.Trace();

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            other.GetComponent<IPlayer>()?.GetDamage(attackPower, this.gameObject);
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
}
