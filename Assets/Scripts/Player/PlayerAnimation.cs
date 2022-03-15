using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Transform _gunTransfom;
    [SerializeField]
    private Transform _gunGripTransfrom;
    [SerializeField]
    private Transform _gunHandGaurdTransfom;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            _animator.SetBool("AimMod", true);
        }
        if(Input.GetMouseButtonUp(1))
        {
            _animator.SetBool("AimMod", false);
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {

        _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        _animator.SetIKPosition(AvatarIKGoal.RightHand, _gunGripTransfrom.position);

        _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        _animator.SetIKPosition(AvatarIKGoal.LeftHand, _gunHandGaurdTransfom.position);

    }

}
