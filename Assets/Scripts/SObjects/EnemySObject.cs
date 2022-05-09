using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Character/Enemy")]

public class EnemySObject : ScriptableObject
{

    // We need:
    // Is Standing Still - bool
    // 
    [SerializeField] private string _name;
    public string Name { get => _name; }

    [SerializeField] private float _moveSpeed;
    public float MoveSpeed { get => _moveSpeed; }

    [SerializeField] private bool _isStandingStill;
    public bool IsStandingStill { get => _isStandingStill; }

    [SerializeField] private bool _canStandPartol;
    public bool CanStandPatrol { get => _canStandPartol; }

    [SerializeField] private float _partolStandingTime;
    public float PatrolStandingTime { get => _partolStandingTime; }
}
