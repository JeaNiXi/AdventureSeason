using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character/Main")]

public class CharacterSObject : ScriptableObject
{
    [SerializeField] private string _name;
    public string Name { get => _name; }

    [SerializeField] private float _health;
    public float Health { get => _health; }

    [SerializeField] private float _baseDamage;
    public float BaseDamage { get => _baseDamage; }

    [SerializeField] private float _heavyDamage;
    public float HeavyDamage { get => _heavyDamage; }

}
