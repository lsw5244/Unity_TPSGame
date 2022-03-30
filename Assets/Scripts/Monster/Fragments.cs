using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragments : MonoBehaviour
{
    public LayerMask fragmentLayer;

    void Start()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, 100f, fragmentLayer);
        for (int i = 0; i < colls.Length; ++i)
        {
            //Destroy(colls[i].gameObject);
            Rigidbody rigi = colls[i].gameObject.GetComponent<Rigidbody>();
            rigi.AddExplosionForce(800f, transform.position, 100f, 10f);
        }

        Destroy(this.gameObject, 5f);
    }
}
