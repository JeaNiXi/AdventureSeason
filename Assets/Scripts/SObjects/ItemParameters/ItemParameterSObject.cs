using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SOModel
{
    [CreateAssetMenu(fileName = "Parameter", menuName = "Item Parameter/Parameter")]

    public class ItemParameterSObject : ScriptableObject
    {
        [field: SerializeField] public string ParameterName { get; private set; }
    }
}