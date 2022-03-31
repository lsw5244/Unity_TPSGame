using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAdder : MonoBehaviour
{
    // Impact
    public void AddBulletExplosion()
    {
        ImpactAbility.Instance.AddBulletExplosion();
    }

    // Hit
    public void AddHitExplosion()
    {
        HitAbility.Instance.AddHitExplosion();
    }

    public void AddHitPoisonDamage()
    {
        HitAbility.Instance.AddHitPoisonDamage();
    }

    // Bullet
    public void AddPoisonBullet()
    {
        BulletAbility.Instance.AddPoisonBullet();
    }


}
