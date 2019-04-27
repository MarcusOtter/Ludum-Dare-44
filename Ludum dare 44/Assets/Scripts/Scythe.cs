using System;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Collider2D))]
public class Scythe : MonoBehaviour
{
    [SerializeField] private string _attackAnimationName;
    [SerializeField] private float _attackDelay = 1;
    private int _attackAnimationHash;

    private Animator _animator;

    private float _lastAttackTimestamp;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _attackAnimationHash = Animator.StringToHash(_attackAnimationName);
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
        _animator.SetTrigger(_attackAnimationHash);
    }

    private void OnDisable()
    {
        InputManager.Instance.OnActionButtonDown -= HandleActionButtonDown;
    }
}
