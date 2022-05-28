using Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Inventory.SOModel
{
    [CreateAssetMenu(fileName ="Edible Item", menuName ="Item/Edible Item")]

    public class ItemEdibleSObject : ItemSObject, IDestoyableItem, IItemAction
    {
        [SerializeField] private List<ModifierData> modifiersData = new List<ModifierData>();

        public string ActionName => "Consume";

        [field: SerializeField] public AudioClip actionSFX { get; private set; }

        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            foreach (ModifierData data in modifiersData)
            {
                data.statModifier.AffectCharacter(character, data.value);
            }
            return true;
        }
    }

    [Serializable]
    public class ModifierData
    {
        public CharacterStatsModifierSObject statModifier;
        public float value;
    }
}