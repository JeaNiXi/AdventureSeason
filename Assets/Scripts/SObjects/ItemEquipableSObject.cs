using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SOModel
{
    [CreateAssetMenu(fileName = "Equipable Item", menuName = "Item/Equipable Item")]

    public class ItemEquipableSObject : ItemSObject, IDestoyableItem, IItemAction
    {
        public string ActionName => "Equip";

        [field: SerializeField] public AudioClip actionSFX { get; private set; }

        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            AgentWeapon weaponSystem = character.GetComponent<AgentWeapon>();
            if (weaponSystem != null)
            {
                weaponSystem.SetWeapon(this, itemState == null ? DefaultParametersList : itemState);
                return true;
            }
            return false;
        }
    }
}