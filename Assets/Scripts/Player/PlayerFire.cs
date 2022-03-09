using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField]
    private Transform _fireTransform;
    [SerializeField]
    private Transform _aimModCameraTransfrom;

    [SerializeField]
    private BulletManager _bulletManager;

    [SerializeField]
    private float _firePower;
    [SerializeField]
    private float _fireDelay;
    [SerializeField]
    private float _fireRayDistance;
    
    private bool _canFire;

    void Start()
    {
        _canFire = true;
    }

    void Update()
    {
        if(Input.GetMouseButton(0) && _canFire == true)
        {
            StartCoroutine("StartDelay");

            if(Input.GetMouseButton(1)) // ���� ����� ��
            {
                RaycastHit hit;

                Vector3 hitPosition = _aimModCameraTransfrom.position + _aimModCameraTransfrom.forward * _fireRayDistance;
                Vector3 hitDirection = (hitPosition - _fireTransform.position).normalized;

                if (Physics.Raycast(_aimModCameraTransfrom.position, _aimModCameraTransfrom.forward, out hit, _fireRayDistance))
                {
                    // ���̿� �¾��� �� ( ���̿� ���� �������� ������)
                    hitDirection = (hit.point - _fireTransform.position).normalized;

                    ShootBullet(hitDirection);
                }
                else
                {
                    // ���̿� �ȸ¾��� �� ( ���̸� �� �������� ������ )
                    ShootBullet(hitDirection);
                }
            }
            else
            {
                ShootBullet(_fireTransform.forward);
            }

        }
    }

    void ShootBullet(Vector3 shootDistance)
    {
        GameObject bullet = _bulletManager.GetBullet();

        if (bullet != null)
        {
            bullet.transform.position = _fireTransform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);

            bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
            bullet.GetComponent<Rigidbody>().AddForce(shootDistance * _firePower);
        }
    }

    IEnumerator StartDelay()
    {
        _canFire = false;
        yield return new WaitForSeconds(_fireDelay);
        _canFire = true;
    }
}