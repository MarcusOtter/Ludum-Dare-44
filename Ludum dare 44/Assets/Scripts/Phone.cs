using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Phone : MonoBehaviour
{
    internal static Phone Instance { get; private set; }

    [Header("Animation parameters")]
    [SerializeField] private string _openPhoneName;
    [SerializeField] private string _closePhoneName;

    private Animator _animator;

    private int _openPhoneHash;
    private int _closePhoneHash;
    private bool _isOpen;

    private void Awake()
    {
        SingletonSetup();

        _animator = GetComponent<Animator>();
        _openPhoneHash = Animator.StringToHash(_openPhoneName);
        _closePhoneHash = Animator.StringToHash(_closePhoneName);
    }

    private void OnEnable()
    {
        InputManager.Instance.OnSpaceDown += Toggle;
    }

    // Used by phone button
    public void GoToApartmentScene()
    {
        SceneTransition.Instance.LoadScene(1);
        Close();
    }

    public void TryPurchaseItem(GameObject item)
    {
        if (InventoryManager.Instance.CanPurchase(item.name))
        {
            InventoryManager.Instance.BuyItem(item.name);
            item.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Close()
    {
        if (_isOpen) 
        {
            _animator.SetTrigger(_closePhoneHash);
            _isOpen = false;
        }
    }

    private void Toggle(object sender, System.EventArgs args)
    {
        // Perfect 72h game jam hack
        if (SceneTransition.Instance.CurrentSceneIndex != 0) 
        {
            Close();
            return; 
        }

        _animator.SetTrigger(_isOpen ? _closePhoneHash : _openPhoneHash);
        _isOpen = !_isOpen;
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

    private void OnDisable()
    {
        InputManager.Instance.OnSpaceDown -= Toggle;
    }
}
