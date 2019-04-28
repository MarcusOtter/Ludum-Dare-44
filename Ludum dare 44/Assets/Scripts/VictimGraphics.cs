using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class VictimGraphics : MonoBehaviour
{
    [SerializeField] private string _soulTransformationName;

    private Animator _animator;
    private Rigidbody2D _rigidbody;

    private int _soulTransformationHash;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _soulTransformationHash = Animator.StringToHash(_soulTransformationName);
    }

    private void OnEnable()
    {
        Victim.OnDeath += (object sender, System.EventArgs args) => 
        {
            TransformToSoul();
        };
    }

    private void TransformToSoul()
    {
        _animator.SetTrigger(_soulTransformationHash);
    }

    private void OnDisable()
    {
        Victim.OnDeath -= (object sender, System.EventArgs args) =>
        {
            TransformToSoul();
        };
    }
}
