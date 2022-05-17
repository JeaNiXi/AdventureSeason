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

    [SerializeField] private float _nearAttackDistance;
    public float NearAttackDistance { get => _nearAttackDistance; }

    [SerializeField] private float _viewDistance;
    public float ViewDistance { get => _viewDistance; }

    [SerializeField] private float _attackInterval;
    public float AttackInterval { get => _attackInterval; }

    [SerializeField] private float _searchDuration;
    public float SearchDuration { get => _searchDuration; }
    [Space]

    [SerializeField] private float _health;
    public float Health { get => _health; }

    [SerializeField] private float _collisionDamage;
    public float CollisionDamage { get => _collisionDamage; }

    [SerializeField] private float _attackDamage;
    public float AttackDamage { get => _attackDamage; }
}
