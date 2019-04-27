using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Victim : MonoBehaviour
{
    internal static event EventHandler OnDeath;
    [SerializeField] internal UnityEvent OnDeathUnityEvent;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var scythe = collider.GetComponent<Scythe>();
        if (scythe == null) { return; }

        print("Victim hit by scythe");

        // play death animation, turn into soul
        OnDeath?.Invoke(this, EventArgs.Empty);
        OnDeathUnityEvent?.Invoke();
    }
}
