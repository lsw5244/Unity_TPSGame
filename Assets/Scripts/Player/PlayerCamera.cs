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
    public float verticalViewAngle = 30f;

    [SerializeField]
    private Transform _cameraTransform;
    [SerializeField]
    private Transform _aimModCameraTransfrom;
    [SerializeField]
    private Transform _normalModCameraTransfrom;

    [SerializeField]
    private Transform _headTransfrom;

    [SerializeField]
    private Image _crosshairImage;  // TODO : UI매니저가 생기면 해당 변수와 기능을 UI매니저에서 하도록 구현하기

    private float _currentVerticalViewAngle = 0;
    private Vector3 _aimModAngle;


    void Update()
    {
        if (Input.GetMouseButton(1)) 
        {
            // 마우스 입력을 토대로 입력 값 저장
            _currentVerticalViewAngle += Input.GetAxis("Mouse Y") * Time.deltaTime * -_cameraMoveSpeed * 2f;
            // 각도 최대, 최소값 제한
            _currentVerticalViewAngle = Mathf.Clamp(_currentVerticalViewAngle, -verticalViewAngle, verticalViewAngle);
            // 입력값을 토대로 바라볼 각도 결정 (X값만 변경하기)
            _aimModAngle = _aimModCameraTransfrom.eulerAngles;
            _aimModAngle.x = _currentVerticalViewAngle;
            _aimModCameraTransfrom.eulerAngles = _aimModAngle;
            //_aimModCameraTransfrom.eulerAngles = new Vector3(temp, _aimModCameraTransfrom.eulerAngles.y, _aimModCameraTransfrom.eulerAngles.z);
            
            // 카메라 이동
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _aimModCameraTransfrom.position, 0.02f);
            _cameraTransform.rotation = _aimModCameraTransfrom.rotation;//Quaternion.Lerp(_cameraTransform.rotation, _aimModCameraTransfrom.rotation, 0.02f);

            _crosshairImage.gameObject.SetActive(true);
        }
        //else if (Input.GetKey(KeyCode.LeftAlt))
        //{
        //    _cameraTransform.position = _normalModCameraTransfrom.position;
        //    // 카메라 피봇 회전
        //    _cameraArmTransform.Rotate(0f
        //        , Input.GetAxis("Mouse X") * _cameraMoveSpeed * Time.deltaTime
        //        , 0f);

        //    _cameraTransform.LookAt(_headTransfrom.position);
        //}
        else if(Input.GetMouseButtonUp(1))
        {
            _crosshairImage.gameObject.SetActive(false);
            // 카메라 회전했던 값 리셋
            _aimModCameraTransfrom.rotation = Quaternion.Euler(0f, 
                _aimModCameraTransfrom.rotation.eulerAngles.y,
                _aimModCameraTransfrom.rotation.eulerAngles.z);
            // 현제 바라보는 각도 초기화
            _currentVerticalViewAngle = 0f;

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
        
        _cameraTransform.LookAt(_headTransfrom.position);
    }
}
