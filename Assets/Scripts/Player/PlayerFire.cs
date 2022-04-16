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
    private AudioSource _audioSource;

    [SerializeField]
    private float _firePower;
    [SerializeField]
    private float _fireDelay;
    [SerializeField]
    private float _fireRayDistance;
    
    private bool _canFire;

    [SerializeField]
    private AudioClip _shootAudio;

    void Start()
    {
        _canFire = true;
        _audioSource.clip = _shootAudio;
    }

    void Update()
    {
        if(Input.GetMouseButton(0) && _canFire == true)
        {
            _audioSource.clip = _shootAudio;
            _audioSource.Play();
            StartCoroutine("StartDelay");

            if(Input.GetMouseButton(1)) // 조준 모드일 때
            {
                RaycastHit hit;

                // Ray에 hit한 것이 없을 때 발사 할 방향 설정
                Vector3 hitPosition = _aimModCameraTransfrom.position + _aimModCameraTransfrom.forward * _fireRayDistance;
                Vector3 hitDirection = (hitPosition - _fireTransform.position).normalized;

                if (Physics.Raycast(_aimModCameraTransfrom.position, _aimModCameraTransfrom.forward, out hit, _fireRayDistance))
                {
                    // 레이에 맞았을 때 ( 레이에 닿은 방향으로 날리기)
                    hitDirection = (hit.point - _fireTransform.position).normalized;

                    ShootBullet(hitDirection);
                }
                else
                {
                    // 레이에 안맞았을 때 ( 레이를 쏜 방향으로 날리기 )
                    ShootBullet(hitDirection);
                }
            }
            else
            {
                ShootBullet(_fireTransform.forward);
            }

        }
    }

    void ShootBullet(Vector3 shootDirection)
    {
        GameObject bullet = _bulletManager.GetBullet();

        if (bullet != null)
        {
            bullet.transform.position = _fireTransform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);

            bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
            bullet.GetComponent<Rigidbody>().AddForce(shootDirection * _firePower);
        }
    }

    IEnumerator StartDelay()
    {
        _canFire = false;
        yield return new WaitForSeconds(_fireDelay);
        _canFire = true;
    }
}
