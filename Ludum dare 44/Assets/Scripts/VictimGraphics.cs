using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class VictimGraphics : MonoBehaviour
{
    [SerializeField] private string _soulTransformationName;
    [SerializeField] private string _getConsumedName = "GetConsumed";

    private Animator _animator;
    private Rigidbody2D _rigidbody;

    private int _soulTransformationHash;
    private int _getConsumedHash;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _soulTransformationHash = Animator.StringToHash(_soulTransformationName);
        _getConsumedHash = Animator.StringToHash(_getConsumedName);
    }

    private void OnEnable()
    {
        Victim.OnDeath += TransformToSoul;
        Victim.OnConsumed += GetConsumed;
    }

    private void TransformToSoul(object sender, System.EventArgs args)
    {
        _animator.SetTrigger(_soulTransformationHash);
    }

    private void GetConsumed(object sender, System.EventArgs args)
    {
        _animator.SetTrigger(_getConsumedHash);
    }

    private void OnDisable()
    {
        Victim.OnDeath -= TransformToSoul;
        Victim.OnConsumed -= GetConsumed;
    }
}
