using System.Collections;
using UnityEngine;

public class ReaperTransformation : MonoBehaviour
{
    internal ReaperForm CurrentForm { get; private set; } = ReaperForm.Physical;
    internal bool IsTransforming { get; private set; }

    [SerializeField] private float _transformationDuration = 3f;

    private void OnEnable()
    {
        Victim.OnDeath += (object sender, System.EventArgs args) =>
        {
            StartCoroutine(Transform());
        };
    }

    private IEnumerator Transform()
    {
        TransformToForm(ReaperForm.Spectral);
        yield return new WaitForSeconds(_transformationDuration);
        IsTransforming = false;
    }

    private void TransformToForm(ReaperForm newReaperForm)
    {
        if (CurrentForm == newReaperForm) { return; }
        CurrentForm = newReaperForm;
        IsTransforming = true;
    }

    private void OnDisable()
    {
        Victim.OnDeath -= (object sender, System.EventArgs args) =>
        {
            StartCoroutine(Transform());
        };
    }
}
