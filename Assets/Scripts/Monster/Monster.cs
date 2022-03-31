using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
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

    public abstract void Idle();
    public abstract void Trace();
    public abstract void Attack();
    public abstract void Die();    
}
