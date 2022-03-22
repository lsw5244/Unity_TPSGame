using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Monster") == false)
        {
            other.gameObject.GetComponent<IPlayer>()?.GetDamage(10f, this.gameObject);
            transform.position = Vector3.zero;
            this.gameObject.SetActive(false);
        }
    }
}
