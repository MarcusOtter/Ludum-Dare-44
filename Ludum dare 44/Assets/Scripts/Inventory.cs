using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Inventory")]
public class Inventory : ScriptableObject
{
    private readonly Dictionary<string, bool> _ownedItems = new Dictionary<string, bool>()
    {
        { "mug(blue)",     false },
        { "mug(red)",      false },
        { "mug(yellow)",   false },
        { "mug(skull)",    false },
        { "plant(small)",  false },
        { "plant(big)",    false },
        { "radio",         false },
        { "poster",        false },
        { "chair(office)", false },
        { "decoscythe",    false },
        { "tie",           false }
    };

    private readonly Dictionary<string, int> _itemPrices = new Dictionary<string, int>()
    {
        { "mug(blue)",     75 },
        { "mug(red)",      75 },
        { "mug(yellow)",   75 },
        { "mug(skull)",    150 },
        { "plant(small)",  200 },
        { "plant(big)",    250 },
        { "radio",         275 },
        { "poster",        300 },
        { "chair(office)", 399 },
        { "decoscythe",    600 },
        { "tie",           2000 }
    };

    private int _soulFragments = 125;

    internal int SoulFragments => _soulFragments;

    internal void AddSoulFragments(int soulFragments)
    {
        _soulFragments += soulFragments;
    }

    internal bool HasItem(string id)
    {
        if (!_ownedItems.ContainsKey(id))
        {
            Debug.LogError("Does not contain key " + id);
            return false;
        }

        return _ownedItems[id];
    }

    internal void BuyItem(string id)
    {
        if (!_itemPrices.ContainsKey(id)) { Debug.LogError("No such id: " + id); }
        _soulFragments -= _itemPrices[id];
        _ownedItems[id] = true;
    }

    internal bool CanPurchase(string id)
    {
        var itemPrice = _itemPrices.FirstOrDefault(x => x.Key == id);

        if (itemPrice.Value == 0)
        {
            Debug.LogError($"'{id}' is not a valid id");
            return false;
        }

        if (HasItem(id)) { return false; }

        return itemPrice.Value <= _soulFragments;
    }
}
