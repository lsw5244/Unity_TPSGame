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

    void Start()
    {
        currentHp = _maxHP;
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _playerTransform = GameObject.Find("Player").transform;/* Find("Player");*/
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, _playerTransform.position);

        /*//Debug.Log(Vector3.Distance(_playerTransform.position, transform.position));
        //if(Vector3.Distance(_playerTransform.position, transform.position) < 3f)
        //{
        //    _navMeshAgent.destination = _playerTransform.position;
        //}
        //if(Input.GetKeyDown(KeyCode.Keypad1))
        //{
        //    _animator.SetBool("Trace", true);
        //}
        //if (Input.GetKeyDown(KeyCode.Keypad2))
        //{
        //    GetDamage(1f);
        //}
        //Debug.Log(currentHp);*/
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
        _animator.SetTrigger("Die");

        _isAlive = false;

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
        Debug.Log("Attack");
        _animator.SetBool("Attack", true);
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
        _navMeshAgent.destination = _playerTransform.position;
    }

    //Idle, Trace, Attack, Die
}
