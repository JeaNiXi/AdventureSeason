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
}
