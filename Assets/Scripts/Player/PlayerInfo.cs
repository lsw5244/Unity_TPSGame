using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour, IPlayer
{
    [SerializeField]
    private GameObject _gun;

    public float maxHp = 100f;

    private float currentHp;

    private bool _isAlive = true;

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _getDamageAudio;
    [SerializeField]
    private AudioClip _dieAudio;

    void Start()
    {        
        currentHp = PlayerPrefs.GetFloat("PlayerHp");
        _audioSource = GetComponent<AudioSource>();
        UIManager.Instance.UpdateHpbar(currentHp / maxHp);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //GetDamage(10f, this.gameObject);
            GetDamage(100f, this.gameObject);
        }
    }

    public void Die()
    {


        GameObject.Find("StageChanger").GetComponent<StageChanger>().PlayerDie();

        GetComponent<PlayerAnimation>().PlayerDie();
        _gun.AddComponent<Rigidbody>();
        _gun.GetComponent<BoxCollider>().enabled = true;
        GetComponent<PlayerCamera>().enabled = false;
        GetComponent<PlayerMove>().enabled = false;
        GetComponent<PlayerRotate>().enabled = false;
        GetComponent<PlayerFire>().enabled = false;
        _isAlive = false;

        _audioSource.clip = _dieAudio;
        _audioSource.Play();
    }

    public void GetDamage(float damage, GameObject attacker)
    {
        if(_isAlive == false)
        {
            return;
        }
        currentHp -= damage;
        UIManager.Instance.UpdateHpbar(currentHp / maxHp);
        PlayerPrefs.SetFloat("PlayerHp", currentHp);

        if (HitAbility.Instance.hitAbility != null /*&& attacker.CompareTag("Monster") == true*/)
        {
            HitAbility.Instance.hitAbility(attacker);
        }
        
        if (currentHp <= 0f)
        {
            Die();
            return;
        }
        _audioSource.clip = _getDamageAudio;
        _audioSource.Play();
    }
}
