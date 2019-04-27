using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EtherealPostProcessing : MonoBehaviour
{
    [Header("Vignette")]
    [SerializeField] private Color _vignetteColor;
    [SerializeField] [Range(0.3f, 1f)] private float _vignetteBaseIntensity = 0.55f;
    [SerializeField] [Range(0f, 0.2f)] private float _vignettePulseStrength = 0.02f;
    [SerializeField] [Range(0f, 20f)] private float _vignettePulseSpeed = 2f;

    [Header("Chromatic aberration")]
    [SerializeField] [Range(0f, 1f)] private float _chromaticAberationBaseIntensity = 0.3f;
    [SerializeField] [Range(0f, 0.2f)] private float _chromaticAberationPulseStrength = 0.015f;
    [SerializeField] [Range(0f, 20f)] private float _chromaticAberationPulseSpeed = 20f;


    private PostProcessVolume _postProcessVolume;

    private Vignette _vignette;
    private ChromaticAberration _chromaticAberration;

    private void Start()
    {
        // Vignette
        _vignette = ScriptableObject.CreateInstance<Vignette>();
        _vignette.enabled.Override(true);
        _vignette.intensity.Override(1f);
        _vignette.color.Override(_vignetteColor);

        // Chromatic aberration
        _chromaticAberration = ScriptableObject.CreateInstance<ChromaticAberration>();
        _chromaticAberration.enabled.Override(true);

        // put this in some kind of coroutine probably
        _postProcessVolume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, _vignette, _chromaticAberration);
    }

    private void Update()
    {
        _vignette.intensity.value = _vignetteBaseIntensity + Mathf.Sin(Time.realtimeSinceStartup * _vignettePulseSpeed) * _vignettePulseStrength;
        _chromaticAberration.intensity.Override(_chromaticAberationBaseIntensity + Mathf.Sin(Time.realtimeSinceStartup * _chromaticAberationPulseSpeed) * _chromaticAberationPulseStrength);
    }

    private void OnDestroy()
    {
        RuntimeUtilities.DestroyVolume(_postProcessVolume, true, true);
    }
}
