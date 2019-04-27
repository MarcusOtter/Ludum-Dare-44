using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class ReaperGraphics : MonoBehaviour
{
    internal bool FacingRight { get; private set; } = true;

    [Header("References")]
    [SerializeField] private SpriteRenderer _bodySpriteRenderer;

    [Header("Animator parameters")]
    [SerializeField] private string _horizontalVelocityName = "AbsVelocityX";

    private Animator _animator;
    private Rigidbody2D _rigidbody;

    private int _horizontalVelocityHash;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _horizontalVelocityHash = Animator.StringToHash(_horizontalVelocityName);
    }

    private void Update()
    {
        _animator.SetFloat(_horizontalVelocityHash, Mathf.Abs(_rigidbody.velocity.x));

        if (_rigidbody.velocity.x < -0.1f)
        {
            FacingRight = false;
            _bodySpriteRenderer.flipX = true;
        }
        else if (_rigidbody.velocity.x > 0.1f)
        {
            FacingRight = true;
            _bodySpriteRenderer.flipX = false;
        }
    }
}
