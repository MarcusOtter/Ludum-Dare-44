using System;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Collider2D))]
public class Scythe : MonoBehaviour
{
    [SerializeField] private string _leftAttackAnimationName = "AttackLeft";
    [SerializeField] private string _rightAttackAnimationName = "AttackRight";
    [SerializeField] private string _facingRightAnimationName = "FacingRight";

    [SerializeField] private float _attackDelay = 1;

    private int _leftAttackAnimationHash;
    private int _rightAttackAnimationHash;
    private int _facingRightAnimationHash;

    private Animator _animator;
    private ReaperGraphics _reaperGraphics;

    private float _lastAttackTimestamp;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _reaperGraphics = GetComponentInParent<ReaperGraphics>();

        _leftAttackAnimationHash = Animator.StringToHash(_leftAttackAnimationName);
        _rightAttackAnimationHash = Animator.StringToHash(_rightAttackAnimationName);
        _facingRightAnimationHash = Animator.StringToHash(_facingRightAnimationName);
    }

    private void Update()
    {
        _animator.SetBool(_facingRightAnimationHash, _reaperGraphics.FacingRight);
    }

    private void OnEnable()
    {
        InputManager.Instance.OnActionButtonDown += HandleActionButtonDown;
    }

    private void HandleActionButtonDown(object sender, EventArgs eventArgs)
    {
        if (_lastAttackTimestamp + _attackDelay <= Time.time)
        {
            Attack();
        }
    }

    private void Attack()
    {
        _lastAttackTimestamp = Time.time;
        _animator.SetTrigger(_reaperGraphics.FacingRight ? _rightAttackAnimationHash : _leftAttackAnimationHash);
    }

    private void OnDisable()
    {
        InputManager.Instance.OnActionButtonDown -= HandleActionButtonDown;
    }
}
