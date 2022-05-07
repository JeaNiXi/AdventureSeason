using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Character/Enemy")]

public class EnemySObject : ScriptableObject
{
    [SerializeField] private string _name;
    public string Name { get => _name; }
    [SerializeField] private bool _canStandPartol;
    public bool CanStandPatrol { get => _canStandPartol; }
    [SerializeField] private float _partolStandingTime;
    public float PatrolStandingTime { get => _partolStandingTime; }
}
