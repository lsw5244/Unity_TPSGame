using UnityEngine;

public interface IPlayer
{
    void GetDamage(float damage, GameObject attacker);

    void Die();
}
