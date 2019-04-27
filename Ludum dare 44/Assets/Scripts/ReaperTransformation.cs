using UnityEngine;

public class ReaperTransformation : MonoBehaviour
{
    internal ReaperForm CurrentForm { get; private set; }
    internal bool IsTransforming { get; private set; }

    private void TransformToForm(ReaperForm newReaperForm)
    {
        if (CurrentForm == newReaperForm) { return; }


    }
}
