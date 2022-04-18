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
    private Image _crosshairImage;  // TODO : UI�Ŵ����� ����� �ش� ������ ����� UI�Ŵ������� �ϵ��� �����ϱ�

    private float _currentVerticalViewAngle = 0;
    private Vector3 _aimModAngle;


    void Update()
    {
        if (Input.GetMouseButton(1)) 
        {
            // ���콺 �Է��� ���� �Է� �� ����
            _currentVerticalViewAngle += Input.GetAxis("Mouse Y") * Time.deltaTime * -_cameraMoveSpeed * 2f;
            // ���� �ִ�, �ּҰ� ����
            _currentVerticalViewAngle = Mathf.Clamp(_currentVerticalViewAngle, -verticalViewAngle, verticalViewAngle);
            // �Է°��� ���� �ٶ� ���� ���� (X���� �����ϱ�)
            _aimModAngle = _aimModCameraTransfrom.eulerAngles;
            _aimModAngle.x = _currentVerticalViewAngle;
            _aimModCameraTransfrom.eulerAngles = _aimModAngle;
            //_aimModCameraTransfrom.eulerAngles = new Vector3(temp, _aimModCameraTransfrom.eulerAngles.y, _aimModCameraTransfrom.eulerAngles.z);
            
            // ī�޶� �̵�
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _aimModCameraTransfrom.position, 0.02f);
            _cameraTransform.rotation = _aimModCameraTransfrom.rotation;//Quaternion.Lerp(_cameraTransform.rotation, _aimModCameraTransfrom.rotation, 0.02f);

            _crosshairImage.gameObject.SetActive(true);
        }
        //else if (Input.GetKey(KeyCode.LeftAlt))
        //{
        //    _cameraTransform.position = _normalModCameraTransfrom.position;
        //    // ī�޶� �Ǻ� ȸ��
        //    _cameraArmTransform.Rotate(0f
        //        , Input.GetAxis("Mouse X") * _cameraMoveSpeed * Time.deltaTime
        //        , 0f);

        //    _cameraTransform.LookAt(_headTransfrom.position);
        //}
        else if(Input.GetMouseButtonUp(1))
        {
            _crosshairImage.gameObject.SetActive(false);
            // ī�޶� ȸ���ߴ� �� ����
            _aimModCameraTransfrom.rotation = Quaternion.Euler(0f, 
                _aimModCameraTransfrom.rotation.eulerAngles.y,
                _aimModCameraTransfrom.rotation.eulerAngles.z);
            // ���� �ٶ󺸴� ���� �ʱ�ȭ
            _currentVerticalViewAngle = 0f;

        }
        else
        {
            ResetCameraTransfrom();
        }
    }

    void ResetCameraTransfrom()
    {
        // ī�޶�Arm ���� ��ġ�� ������
        _cameraArmTransform.localRotation = Quaternion.Lerp(_cameraArmTransform.localRotation,
            Quaternion.identity, 0.02f);

        // ī�޶� ��ġ ���� ��ġ�� ������
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _normalModCameraTransfrom.position, 0.02f);
        
        _cameraTransform.LookAt(_headTransfrom.position);
    }
}
