using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    private float _horizontal;

    [SerializeField]
    private float _rotateSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        _horizontal = Input.GetAxis("Mouse X");

        transform.Rotate(0f, _horizontal, 0f);
    }
}
