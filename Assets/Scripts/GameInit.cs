using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    [SerializeField]
    private PlayerInfo _playerInfo;

    private void Awake()
    {
        PlayerPrefs.SetFloat("PlayerHp", _playerInfo.maxHp);

        BulletAbility.Instance?.ClearAbility();
        ImpactAbility.Instance?.ClearAbility();
        HitAbility.Instance?.ClearAbility();
    }
}
