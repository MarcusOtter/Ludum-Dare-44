using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Collider2D))]
public class SoulEater : MonoBehaviour
{
    [SerializeField] private string SoulInRangeName;
    [SerializeField] private string EatSoulName;

    private Animator _animator;
    
    //private int 

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }
}
