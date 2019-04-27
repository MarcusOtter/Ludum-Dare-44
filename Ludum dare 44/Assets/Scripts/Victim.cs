using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Victim : MonoBehaviour
{
    internal static event EventHandler OnDeath;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var scythe = collider.GetComponent<Scythe>();
        if (scythe == null) { return; }

        print("Victim hit by scythe");

        // play death animation, turn into soul
        OnDeath?.Invoke(this, EventArgs.Empty);
    }
}
