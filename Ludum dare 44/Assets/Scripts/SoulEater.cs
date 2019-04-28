using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Collider2D))]
public class SoulEater : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _biteCooldown;
    //[SerializeField] private float _biteTimeScale = 0.2f;
    //[SerializeField] private float _slowMotionDuration = 10f;

    [Header("Animation parameters")]
    [SerializeField] private string _soulInRangeName;
    [SerializeField] private string _eatSoulName;

    private Animator _animator;

    private int _soulInRangeHash;
    private int _eatSoulHash;

    private Victim _soulInRange;
    private float _lastBiteTime;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _soulInRangeHash = Animator.StringToHash(_soulInRangeName);
        _eatSoulHash = Animator.StringToHash(_eatSoulName);
    }

    private void OnEnable()
    {
        InputManager.Instance.OnActionButtonDown += TryToEatSoul;

        Victim.OnConsumed += (object sender, System.EventArgs args) =>
        {
            _animator.SetBool(_soulInRangeHash, false);
            this.enabled = false;
        };
    }

    private void TryToEatSoul(object sender, System.EventArgs args)
    {
        if (_lastBiteTime + _biteCooldown > Time.time) { return; }
        //StartCoroutine(SlowMotion());
        _lastBiteTime = Time.time;
        _animator.SetTrigger(_eatSoulHash);
        _soulInRange?.ConsumeSoul();
    }

    //private IEnumerator SlowMotion()
    //{
    //    Time.timeScale = _biteTimeScale;
    //    yield return new WaitForSeconds(_slowMotionDuration);
    //    Time.timeScale = 1f;
    //}

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Soul")) { return; }
        _soulInRange = collider.GetComponentInParent<Victim>();
        _animator.SetBool(_soulInRangeHash, true);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (_soulInRange is null) { return; }
        if (!collider.CompareTag("Soul")) { return; }
        _soulInRange = null;
        _animator.SetBool(_soulInRangeHash, false);
    }

    private void OnDisable()
    {
        InputManager.Instance.OnActionButtonDown -= TryToEatSoul;

        Victim.OnConsumed -= (object sender, System.EventArgs args) =>
        {
            this.enabled = false;
        };
    }
}
