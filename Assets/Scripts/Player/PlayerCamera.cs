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
    private Image _crosshairImage;  // TODO : UI�Ŵ����� ����� �ش� ������ ����� UI�Ŵ������� �ϵ��� �����ϱ�
    
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
        // ī�޶�Arm ���� ��ġ�� ������
        _cameraArmTransform.localRotation = Quaternion.Lerp(_cameraArmTransform.localRotation,
            Quaternion.identity, 0.02f);

        // ī�޶� ��ġ ���� ��ġ�� ������
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _normalModCameraTransfrom.position, 0.02f);
        
        _cameraTransform.LookAt(transform.position);
    }
}
