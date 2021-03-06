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

    public void StartPoisonEffect(float damage)
    {
        poisonDamageCount = 5;

        if (_isPoisonState == false)
        {
            StartCoroutine(Poison(damage));
        }
    }

    public override void Die()
    {
        base.Die();
        
        _leftHandFireParticle.SetActive(false);
        _rightHandFireParticle.SetActive(false);

        GetComponent<CapsuleCollider>().enabled = false;
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
