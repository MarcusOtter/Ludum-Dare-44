using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Victim : MonoBehaviour
{
    internal static event EventHandler OnDeath;
    internal static event EventHandler OnSoulReleased;
    [SerializeField] internal UnityEvent OnDeathUnityEvent;

    [Header("Settings")]
    [SerializeField] private float _timeUntilSoulReleased = 1.5f;
    [SerializeField] private float _soulMovementSpeed;
    [SerializeField] private float _soulRotationSpeed;
    [SerializeField] private float _minTimeUntilNewRotation = 0.1f;
    [SerializeField] private float _maxTimeUntilNewRotation = 0.5f;

    private Rigidbody2D _rigidbody;
    private VictimGraphics _victimGraphics;

    private bool _insideBoundary;
    private bool _isEscaping;
    private Quaternion _targetRotation;
    private Coroutine _updateTargetRotationCoroutine;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void Update()
    {
        if (!_isEscaping) { return; }

        var slerpedTargetRotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * _soulRotationSpeed);

        _rigidbody.SetRotation(slerpedTargetRotation);
        _rigidbody.velocity = transform.up * _soulMovementSpeed;
    }

    // internal void getconsumed, call event

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("SoulBoundary"))
        {
            if (_updateTargetRotationCoroutine != null)
            {
                StopCoroutine(_updateTargetRotationCoroutine);
                _updateTargetRotationCoroutine = null;
            }

            _targetRotation = Quaternion.AngleAxis(180f, Vector3.forward) * transform.rotation;
        }

        var scythe = collider.GetComponent<Scythe>();
        if (scythe == null) { return; }

        print("Victim hit by scythe");

        OnDeath?.Invoke(this, EventArgs.Empty);
        OnDeathUnityEvent?.Invoke();

        StartCoroutine(ReleaseSoulDelayed(_timeUntilSoulReleased));
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("SoulBoundary"))
        {
            if (_updateTargetRotationCoroutine != null) { StopCoroutine(_updateTargetRotationCoroutine); }
            _updateTargetRotationCoroutine = StartCoroutine(UpdateTargetRotation());
        }
    }

    private IEnumerator ReleaseSoulDelayed(float delay)
    {
        if (_updateTargetRotationCoroutine != null) { StopCoroutine(_updateTargetRotationCoroutine); }
        _updateTargetRotationCoroutine = StartCoroutine(UpdateTargetRotation());

        yield return new WaitForSeconds(delay);
        _rigidbody.SetRotation(Quaternion.Euler(0f, 0f, 30f));
        OnSoulReleased?.Invoke(this, EventArgs.Empty);

        _isEscaping = true;
        _rigidbody.constraints = RigidbodyConstraints2D.None;
    }

    private IEnumerator UpdateTargetRotation()
    {
        yield return new WaitForSeconds(0.5f);

        while (_isEscaping)
        {
            var timeUntilNewRotation = Random.Range(_minTimeUntilNewRotation, _maxTimeUntilNewRotation);
            var randomOffsetDegrees = Random.Range(-45, 90);

            _targetRotation = Quaternion.AngleAxis(randomOffsetDegrees, Vector3.forward) * transform.rotation;

            yield return new WaitForSeconds(timeUntilNewRotation);
        }
    }
}
