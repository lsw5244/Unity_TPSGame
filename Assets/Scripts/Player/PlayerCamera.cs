using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCamera : MonoBehaviour
{
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
    [SerializeField]
    private Image _crosshairImage;  // TODO : UI매니저가 생기면 해당 변수와 기능을 UI매니저에서 하도록 구현하기
    
    void Update()
    {
        if (Input.GetMouseButton(1)) 
        {
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _aimModCameraTransfrom.position, 0.02f);
            _cameraTransform.rotation = Quaternion.Lerp(_cameraTransform.rotation, _aimModCameraTransfrom.rotation, 0.02f);

            _crosshairImage.gameObject.SetActive(true);
        }
        else if (Input.GetKey(KeyCode.LeftAlt))
        {
            _cameraArmTransform.Rotate(/*Input.GetAxis("Mouse Y") * -_cameraMoveSpeed * Time.deltaTime*/0f
                , Input.GetAxis("Mouse X") * _cameraMoveSpeed * Time.deltaTime
                , 0f);

            _cameraTransform.LookAt(transform.position);
        }
        else if(Input.GetMouseButtonUp(1))
        {
            _crosshairImage.gameObject.SetActive(false);
        }
        else
        {
            ResetCameraTransfrom();
        }
    }

    void ResetCameraTransfrom()
    {
        // 카메라Arm 원래 위치로 돌리기
        _cameraArmTransform.localRotation = Quaternion.Lerp(_cameraArmTransform.localRotation,
            Quaternion.identity, 0.02f);

        // 카메라 위치 원래 위치로 돌리기
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _normalModCameraTransfrom.position, 0.02f);
        
        _cameraTransform.LookAt(transform.position);
    }
}
