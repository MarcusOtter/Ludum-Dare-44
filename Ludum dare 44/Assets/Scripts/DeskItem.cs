using UnityEngine;

public class DeskItem : MonoBehaviour
{
    [SerializeField] internal string Id;
    [SerializeField] internal int Price;

    private InventoryManager _inventoryManager;

    private void Start()
    {
        _inventoryManager = InventoryManager.Instance;
        UpdateVisibility();
    }

    internal void UpdateVisibility()
    {
        gameObject.SetActive(_inventoryManager.HasItem(Id));
    }
}
