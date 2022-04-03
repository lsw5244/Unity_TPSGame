using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RangedMonster : Monster, IMonster
{
    public float attackPower = 10f;

    [SerializeField]
    private GameObject _leftHandFireParticle;
    [SerializeField]
    private GameObject _rightHandFireParticle;

    [SerializeField]
    private BulletManager _bulletManager;
    [SerializeField]
    private Transform _fireTransform;
    [SerializeField]
    private float _firePower = 1000f;

    public void TurnOnAttackFire()
    {
        _leftHandFireParticle.SetActive(true);
    }

    public void TurnOffAttackFire()
    {
        _leftHandFireParticle.SetActive(false);
    }

    public void ShootFire()
    {
        GameObject bullet = _bulletManager.GetFireBall();

        if (bullet != null)
        {
            bullet.transform.position = _fireTransform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);

            bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;

            Vector3 shootDirection = (_playerTransform.transform.position - transform.position).normalized;
            bullet.GetComponent<Rigidbody>().AddForce(shootDirection * _firePower);
        }
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        currentHp = _maxHP;

        _playerTransform = GameObject.Find("Player").transform;/* Find("Player");*/
        StartCoroutine(StateCheck());
    }

    IEnumerator StateCheck()
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

    public override void Die()
    {
        _isAlive = false;

        StopAllCoroutines();
        _poisonParicle.SetActive(false);

        _animator.SetTrigger("Die");

        _navMeshAgent.isStopped = true;
        _navMeshAgent.velocity = Vector3.zero;

        _leftHandFireParticle.SetActive(false);
        _rightHandFireParticle.SetActive(false);

        GetComponent<CapsuleCollider>().enabled = false;

        GameObject.Find("StageChanger").GetComponent<StageChanger>().RemoveMonsterCount();
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
        base.Idle();

        _leftHandFireParticle.SetActive(false);
    }

    public override void Trace()
    {
        base.Trace();

        _leftHandFireParticle.SetActive(false);
    }

    public override void Attack()
    {
        // 애니메이션 변경
        _animator.SetBool("Trace", true);
        _animator.SetBool("Attack", true);

        transform.LookAt(_playerTransform.position);

        // 추적 중지
        _navMeshAgent.isStopped = true;
        _navMeshAgent.velocity = Vector3.zero;
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
}
