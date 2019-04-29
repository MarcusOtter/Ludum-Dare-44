using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory")]
public class Inventory : ScriptableObject
{
    [SerializeField] private List<GameObject> gameObjects;
}
