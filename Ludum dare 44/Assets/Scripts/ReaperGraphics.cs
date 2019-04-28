using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(ReaperTransformation))]
public class ReaperGraphics : MonoBehaviour
{
    internal bool FacingRight { get; private set; } = true;

    [Header("References")]
    [SerializeField] private SpriteRenderer _bodySpriteRenderer;
    [SerializeField] private SpriteRenderer _headSpriteRenderer;

    [Header("Animator parameters")]
    [SerializeField] private string _horizontalVelocityName = "AbsVelocityX";
    [SerializeField] private string _transformToSpectralName = "TransformToSpectral";

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private ReaperTransformation _reaperTransformation;

    private int _horizontalVelocityHash;
    private int _transformToSpectralHash;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _reaperTransformation = GetComponent<ReaperTransformation>();

        _horizontalVelocityHash = Animator.StringToHash(_horizontalVelocityName);
        _transformToSpectralHash = Animator.StringToHash(_transformToSpectralName);
    }

    private void OnEnable()
    {
        Victim.OnDeath += (object sender, System.EventArgs args)
            => _animator.SetTrigger(_transformToSpectralHash);
    }

    private void Update()
    {
        _animator.SetFloat(_horizontalVelocityHash, Mathf.Abs(_rigidbody.velocity.x));

        if (_reaperTransformation.CurrentForm == ReaperForm.Spectral)
        {
            _bodySpriteRenderer.flipY = (transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270);
            _headSpriteRenderer.flipY = (transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270);
            return;
        }

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

    private void OnDisable()
    {
        Victim.OnDeath -= (object sender, System.EventArgs args)
            => _animator.SetTrigger(_transformToSpectralHash);
    }
}
