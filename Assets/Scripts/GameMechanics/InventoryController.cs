using Interfaces;
using Inventory.SOModel;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        
        public static InventoryController Instance;

        [SerializeField] public MouseFollower mouseFollower;
        [SerializeField] public InventorySObject inventoryData;
        [SerializeField] private UIInventoryPage UIInventoryPage;

        public List<InventoryItem> initialItems = new List<InventoryItem>();

        [SerializeField] private AudioClip dropClip;
        [SerializeField] private AudioSource audioSource;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }

            PrepareUI();
            PrepareInventoryData();

            if (UIInventoryPage.isActiveAndEnabled)
            {
                UIInventoryPage.Hide();
            }
            if (mouseFollower.isActiveAndEnabled)
            {
                mouseFollower.Toggle(false);
            }
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItem item in initialItems)
            {
                if (item.IsEmpty)
                    continue;
                inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            UIInventoryPage.ResetAllItems();
            foreach (var item in inventoryState)
            {
                UIInventoryPage.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
            }
        }
        private void PrepareUI()
        {
            UIInventoryPage.InitializeInventoryUI(inventoryData.Size);
            UIInventoryPage.OnDescriptionRequested += HandleDescriptionRequest;
            UIInventoryPage.OnSwapItems += HandleSwapItems;
            UIInventoryPage.OnStartDragging += HandleDragging;
            UIInventoryPage.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            IItemAction itemAction = inventoryItem.item as IItemAction;
            if(itemAction!=null)
            {
                UIInventoryPage.ShowItemAction(itemIndex);
                UIInventoryPage.AddAction(itemAction.ActionName, () => PerformAction(itemIndex));
            }
            IDestoyableItem destroyableItem = inventoryItem.item as IDestoyableItem;
            if (destroyableItem != null)
            {
                UIInventoryPage.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.quantity));
            }
        }
        private void DropItem(int itemIndex, int quantity)
        {
            inventoryData.RemoveItem(itemIndex, quantity);
            UIInventoryPage.ResetSelection();
            audioSource.PlayOneShot(dropClip);
        }
        public void PerformAction(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;

            IDestoyableItem destroyableItem = inventoryItem.item as IDestoyableItem;
            if (destroyableItem != null)
            {
                Debug.Log("Item Destroyed");
                inventoryData.RemoveItem(itemIndex, 1);
            }

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                Debug.Log("Action performed");
                itemAction.PerformAction(GameManager.Instance.mainCharacter, inventoryItem.itemState);
                audioSource.PlayOneShot(itemAction.actionSFX);
                //if (inventoryData.GetItemAt(itemIndex).IsEmpty)
                    UIInventoryPage.ResetSelection();
            }
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            UIInventoryPage.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                UIInventoryPage.ResetSelection();
                return;
            }
            ItemSObject item = inventoryItem.item;
            string description = PrepareDescription(inventoryItem);
            UIInventoryPage.UpdateDescription(itemIndex, item.ItemImage, item.name, description);
        }

        public string PrepareDescription(InventoryItem inventoryItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(inventoryItem.item.Description);
            sb.AppendLine();
            for (int i = 0; i < inventoryItem.itemState.Count; i++)
            {
                sb.Append($"{inventoryItem.itemState[i].itemParameter.ParameterName} " + $": {inventoryItem.itemState[i].value} / {inventoryItem.item.DefaultParametersList[i].value}");
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private void Start()
        {

        }
        private void Update()
        {

        }

        public void ToggleInventory()
        {
            if (UIInventoryPage.isActiveAndEnabled == false)
            {
                Debug.Log("InvController Showing");
                UIInventoryPage.Show();
                foreach (var item in inventoryData.GetCurrentInventoryState())
                {
                    UIInventoryPage.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
                }
            }
            else
            {
                Debug.Log("InvController Hiding");
                UIInventoryPage.Hide();
            }
        }
    }
}