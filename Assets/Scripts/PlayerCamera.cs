using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraArmTransform;

    [SerializeField]
    private float _cameraMoveSpeed = 200f;

    [SerializeField]
    private Transform _camera;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            _cameraArmTransform.Rotate(Input.GetAxis("Mouse Y") * _cameraMoveSpeed * Time.deltaTime
                , Input.GetAxis("Mouse X") * _cameraMoveSpeed * Time.deltaTime
                , 0f);
        }
        else if(Input.GetMouseButton(1))
        {

        }
        else
        {
            _cameraArmTransform.rotation = Quaternion.Lerp(_cameraArmTransform.rotation,
                 Quaternion.identity, 0.1f);    // 카메라 원래 위치로 돌리기
        }

        _camera.LookAt(transform.position);
    }
}
