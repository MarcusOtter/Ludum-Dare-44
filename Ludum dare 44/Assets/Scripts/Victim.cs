using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Victim : MonoBehaviour
{
    private static int KillCount = 1;

    internal static event EventHandler OnDeath;
    internal static event EventHandler OnSoulReleased;
    internal static event EventHandler OnConsumed;
    [SerializeField] internal UnityEvent OnDeathUnityEvent;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI _rewardAmountText;
    [SerializeField] private GameObject _toEnable;

    [Header("Settings")]
    [SerializeField] private float _timeUntilSoulReleased = 1.5f;
    [SerializeField] private float _soulMovementSpeed;
    [SerializeField] private float _soulRotationSpeed;
    [SerializeField] private float _minTimeUntilNewRotation = 0.1f;
    [SerializeField] private float _maxTimeUntilNewRotation = 0.5f;
    [SerializeField] private float _startRotationZ = 0f;
    [SerializeField] private int _baseReward = 200;

    private int _currentReward;

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

        _currentReward = (int) (_baseReward * Mathf.Pow(1.25f, KillCount));

        _soulMovementSpeed *= Mathf.Pow(1.1f, KillCount);
        _soulRotationSpeed *= Mathf.Pow(1.1f, KillCount);
    }

    private void Update()
    {
        if (!_isEscaping) { return; }

        var slerpedTargetRotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * _soulRotationSpeed);

        _rigidbody.SetRotation(slerpedTargetRotation);
        _rigidbody.velocity = transform.up * _soulMovementSpeed;
    }

    internal void ConsumeSoul()
    {
        _isEscaping = false;
        OnConsumed?.Invoke(this, EventArgs.Empty);
        print("Soul consumed!");
        _rigidbody.velocity = Vector2.zero;
        KillCount++;
        print("adding " + _baseReward * KillCount);
        InventoryManager.Instance.AddSoulFragments(_currentReward);
        this.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("SoulBoundary") && _isEscaping)
        {
            if (_updateTargetRotationCoroutine != null)
            {
                StopCoroutine(_updateTargetRotationCoroutine);
                _updateTargetRotationCoroutine = null;
            }

            _targetRotation = Quaternion.AngleAxis(180f, Vector3.forward) * transform.rotation;
            //print("Rotated 180 degrees");
        }

        var scythe = collider.GetComponent<Scythe>();
        if (scythe is null) { return; }

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
        _rigidbody.SetRotation(Quaternion.Euler(0f, 0f, _startRotationZ));
        OnSoulReleased?.Invoke(this, EventArgs.Empty);

        _isEscaping = true;
        _rigidbody.constraints = RigidbodyConstraints2D.None;
        _toEnable.SetActive(true);
        StartCoroutine(DecreaseRewardAmount());
    }

    private IEnumerator DecreaseRewardAmount()
    {
        while (_isEscaping)
        {
            if (_currentReward < 0)
            {
                if (KillCount > 1) KillCount--;
                ConsumeSoul();
                break;
            }

            _currentReward -= 1;
            _rewardAmountText.text = _currentReward.ToString();
            yield return new WaitForSeconds(0.1f - 0.02f * KillCount);
        }
    }

    private IEnumerator UpdateTargetRotation()
    {
        yield return new WaitForSeconds(0.5f);

        while (_isEscaping)
        {
            var timeUntilNewRotation = Random.Range(_minTimeUntilNewRotation, _maxTimeUntilNewRotation);
            var randomOffsetDegrees = Random.Range(-45f, 45f);

            _targetRotation = Quaternion.AngleAxis(randomOffsetDegrees, Vector3.forward) * transform.rotation;

            yield return new WaitForSeconds(timeUntilNewRotation);
        }
    }
}
