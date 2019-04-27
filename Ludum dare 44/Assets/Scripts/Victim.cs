using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Victim : MonoBehaviour
{
    internal static event EventHandler OnDeath;
    [SerializeField] internal UnityEvent OnDeathUnityEvent;

    [Header("Settings")]
    [SerializeField] private float _timeUntilSoulReleased = 1.5f;

    private Rigidbody2D _rigidbody;
    private VictimGraphics _victimGraphics;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var scythe = collider.GetComponent<Scythe>();
        if (scythe == null) { return; }

        print("Victim hit by scythe");

        OnDeath?.Invoke(this, EventArgs.Empty);
        OnDeathUnityEvent?.Invoke();

        // Do escape behavior when the animation finishes
        _rigidbody.constraints = RigidbodyConstraints2D.None;
    }
}
