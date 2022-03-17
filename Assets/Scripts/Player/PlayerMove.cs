using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;

    private Transform _playerTransfrom;
    private Rigidbody _playerRigidbody;

    private float _horizontal;
    private float _vertical;

    void Start()
    {
        _playerTransfrom = GetComponent<Transform>();
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        Vector3 moveFoward = _vertical * Time.deltaTime * moveSpeed * transform.forward; // ��, �ڷ� �����̴� ����
        Vector3 moveRight = _horizontal * Time.deltaTime * moveSpeed * transform.right;  // ��, ��� �����̴� ����

        //transform.position += moveFoward + moveRight;

        //_playerRigidbody.velocity += moveFoward + moveRight;

        _playerRigidbody.MovePosition(_playerRigidbody.position + moveFoward + moveRight);
    }
}
