using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    internal static InventoryManager Instance { get; private set; }

    private void Awake()
    {
        SingletonSetup();
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
