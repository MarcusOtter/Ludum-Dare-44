using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas), typeof(Animator))]
public class SceneTransition : MonoBehaviour
{
    internal static SceneTransition Instance { get; private set; }
    
    internal int CurrentSceneIndex => SceneManager.GetActiveScene().buildIndex;

    [Header("Animator parameters")]
    [SerializeField] private string _enableLoadingScreenName;
    [SerializeField] private string _disableLoadingScreenName;

    [Header("Animation durations")]
    [SerializeField] private float _enableLoadingScreenDuration = 1f;
    [SerializeField] private float _disableLoadingScreenDelay = 1f;
    [SerializeField] private float _soulConsumedDelay = 3f;

    [Header("Other settings")]
    [SerializeField] private int _officeSceneIndex = 0;

    private Animator _animator;

    private int _enableLoadingScreenHash;
    private int _disableLoadingScreenHash;

    private Coroutine _sceneTransitionCoroutine;

    private void Awake()
    {
        SingletonSetup();

        _animator = GetComponent<Animator>();
        _enableLoadingScreenHash = Animator.StringToHash(_enableLoadingScreenName);
        _disableLoadingScreenHash = Animator.StringToHash(_disableLoadingScreenName);
    }

    private void OnEnable()
    {
        Victim.OnConsumed += StartChangeSceneCoroutine;
    }

    // Used by phone button
    internal void LoadScene(int sceneIndex)
    {
        if (_sceneTransitionCoroutine != null) { return; }
        _sceneTransitionCoroutine = StartCoroutine(ChangeScenesTo(sceneIndex));
    }

    internal void LoadNextScene()
    {
        if (_sceneTransitionCoroutine != null) { return; }
        _sceneTransitionCoroutine = StartCoroutine(ChangeScenesTo(SceneManager.GetActiveScene().buildIndex + 1));
    }

    internal void ReloadScene()
    {
        if (_sceneTransitionCoroutine != null) { return; }
        _sceneTransitionCoroutine = StartCoroutine(ChangeScenesTo(SceneManager.GetActiveScene().buildIndex));
    }

    private void StartChangeSceneCoroutine(object sender, System.EventArgs args)
    {
        StartCoroutine(ChangeScenesTo(_officeSceneIndex, _soulConsumedDelay));
    }

    private IEnumerator ChangeScenesTo(int sceneIndex, float delay = 0f)
    {
        if (delay != 0)
        {
            yield return new WaitForSeconds(delay);
        }

        _animator.SetTrigger(_enableLoadingScreenHash);
        yield return new WaitForSeconds(_enableLoadingScreenDuration);
        SceneManager.LoadScene(sceneIndex);
        yield return new WaitForSeconds(_disableLoadingScreenDelay);
        _animator.SetTrigger(_disableLoadingScreenHash);
        _sceneTransitionCoroutine = null;
    }

    private void OnDisable()
    {
        Victim.OnConsumed -= StartChangeSceneCoroutine;
    }

    private void SingletonSetup()
    {
        if (Instance is null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
