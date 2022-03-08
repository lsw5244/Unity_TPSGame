using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    //public Transform _cameraArmTransform;

    //public float _cameraMoveSpeed = 200f;

    //public Transform _cameraTransform;

    //public Transform _aimModCameraTransfrom;

    //public Transform _normalModCameraTransfrom;

    [SerializeField]
    private Transform _cameraArmTransform;
    [SerializeField]
    private float _cameraMoveSpeed = 200f;
    [SerializeField]
    private Transform _cameraTransform;
    [SerializeField]
    private Transform _aimModCameraTransfrom;
    [SerializeField]
    private Transform _normalModCameraTransfrom;

    void Start()
    {
        //_cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.LeftAlt))
        {
            _cameraArmTransform.Rotate(/*Input.GetAxis("Mouse Y") * -_cameraMoveSpeed * Time.deltaTime*/0f
                , Input.GetAxis("Mouse X") * _cameraMoveSpeed * Time.deltaTime
                , 0f);

            _cameraTransform.LookAt(transform.position);
        }
        else if(Input.GetMouseButton(1))
        {
            //_cameraTransform.position = Vector3.zero;

            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _aimModCameraTransfrom.position, 0.02f);
            _cameraTransform.rotation = Quaternion.Lerp(_cameraTransform.rotation, _aimModCameraTransfrom.rotation, 0.02f);
        }
        else
        {
            ResetCameraTransfrom();
        }
    }

    void ResetCameraTransfrom()
    {
        //_cameraArmTransform.rotation = Quaternion.Lerp(_cameraArmTransform.rotation,
        //    transform.rotation, 0.1f);

        // 카메라Arm 원래 위치로 돌리기
        _cameraArmTransform.localRotation = Quaternion.Lerp(_cameraArmTransform.localRotation,
            Quaternion.identity, 0.02f);

        // 카메라 위치 원래 위치로 돌리기
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _normalModCameraTransfrom.position, 0.02f);
        
        _cameraTransform.LookAt(transform.position);
    }
}
