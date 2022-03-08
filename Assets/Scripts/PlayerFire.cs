using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField]
    private Transform _fireTransform;

    [SerializeField]
    private BulletManager _bulletManager;

    [SerializeField]
    private float _firePower;

    [SerializeField]
    private float _fireDelay;

    private bool _canFire;

    [SerializeField]
    private Transform _cameraAimModPos;

    [SerializeField]
    private float _fireRayDistance;

    void Start()
    {
        _canFire = true;
    }

    void Update()
    {
        if(Input.GetMouseButton(0) && _canFire == true)
        {
            StartCoroutine("FireDelay");

            if(Input.GetMouseButton(1)) // ���� ����� ��
            {
                RaycastHit hit;

                Vector3 hitPosition = _cameraAimModPos.position + _cameraAimModPos.forward * _fireRayDistance;
                Vector3 hitDirection = (hitPosition - _fireTransform.position).normalized;

                if (Physics.Raycast(_cameraAimModPos.position, _cameraAimModPos.forward, out hit, _fireRayDistance))
                {
                    // ���̿� �¾��� ��
                    hitDirection = (hit.point - _fireTransform.position).normalized;

                    GameObject bullet = _bulletManager.GetBullet();

                    bullet.transform.position = _fireTransform.position;
                    bullet.transform.rotation = transform.rotation;
                    bullet.SetActive(true);
                    bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    bullet.GetComponent<Rigidbody>().AddForce(hitDirection * _firePower);
                }
                else
                {
                    // ���̿� �ȸ¾��� �� ( ���̸� �� �������� ������ )
                    GameObject bullet = _bulletManager.GetBullet();

                    bullet.transform.position = _fireTransform.position;
                    bullet.transform.rotation = transform.rotation;
                    bullet.SetActive(true);
                    bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    bullet.GetComponent<Rigidbody>().AddForce(hitDirection * _firePower);
                }
            }
            else
            {
                BulletFire();
            }

        }
    }

    void BulletFire()
    {
        GameObject bullet = _bulletManager.GetBullet();

        if (bullet != null)
        {
            bullet.transform.position = _fireTransform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);
            bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * _firePower);
        }
    }

    IEnumerator FireDelay()
    {
        _canFire = false;
        yield return new WaitForSeconds(_fireDelay);
        _canFire = true;
    }
}
