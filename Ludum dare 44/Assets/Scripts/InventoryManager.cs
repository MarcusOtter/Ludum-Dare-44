using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    internal static InventoryManager Instance { get; private set; }

    [SerializeField] private Inventory _inventory;
    [SerializeField] private TextMeshProUGUI _soulShopAmount;
    [SerializeField] private TextMeshProUGUI _soulBankAmount;
    [SerializeField] private GameObject _itemList;
    [SerializeField] private List<DeskItem> _allDeskItems;

    private void Awake()
    {
        SingletonSetup();
        UpdateText();
    }

    internal bool HasItem(string itemId) => _inventory.HasItem(itemId);
    internal bool CanPurchase(string itemId) => _inventory.CanPurchase(itemId);

    private void Update()
    {
        _itemList.SetActive(SceneTransition.Instance.CurrentSceneIndex == 0);
    }

    internal void BuyItem(string itemId)
    {
        _inventory.BuyItem(itemId);
        UpdateItemGraphics();
        UpdateText();
        print("player bought item");
    }

    internal void AddSoulFragments(int soulFragments)
    {
        _inventory.AddSoulFragments(soulFragments);
        UpdateText();
    }

    private void UpdateItemGraphics()
    {
        foreach(var item in _allDeskItems)
        {
            item.UpdateVisibility();
        }
    }

    private void UpdateText()
    {
        _soulShopAmount.text = _inventory.SoulFragments.ToString();
        _soulBankAmount.text = _inventory.SoulFragments.ToString();
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
