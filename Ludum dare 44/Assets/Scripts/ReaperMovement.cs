using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(ReaperTransformation))]
public class ReaperMovement : MonoBehaviour
{
    [SerializeField] private float _normalMovementSpeed = 5;
    [SerializeField] private float _spectralMovementSpeed = 10;

    private InputManager _inputManager;
    private Rigidbody2D _rigidbody;
    private ReaperTransformation _reaperTransformation;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _reaperTransformation = GetComponent<ReaperTransformation>();
    }

    private void OnEnable()
    {
        Victim.OnConsumed += DisableRigidbody;
    }

    private void Start()
    {
        _inputManager = InputManager.Instance;
    }

    private void FixedUpdate()
    {
        if (_reaperTransformation.IsTransforming)
        {
            _rigidbody.velocity = Vector2.zero;
            return;
        }

        switch (_reaperTransformation.CurrentForm)
        {
            case ReaperForm.Physical:
                _rigidbody.velocity = new Vector2(_inputManager.HorizontalAxisValue, 0).normalized * _normalMovementSpeed;
                break;

            case ReaperForm.Spectral:
                _rigidbody.velocity = new Vector2(_inputManager.HorizontalAxisValue, _inputManager.VerticalAxisValue).normalized * _spectralMovementSpeed;
                Vector2 lookDirection = (_inputManager.MouseWorldPosition - transform.position).normalized;
                transform.right = lookDirection;
                break;
        }
    }

    private void DisableRigidbody(object sender, System.EventArgs args)
    {
        _rigidbody.velocity = Vector2.zero;
        this.enabled = false;
    }

    private void OnDisable()
    {
        Victim.OnConsumed -= DisableRigidbody;
    }
}
