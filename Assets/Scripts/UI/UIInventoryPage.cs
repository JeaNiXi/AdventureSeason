using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryPage : MonoBehaviour
{
    [SerializeField] private UIInventoryItem itemPrefab;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private UIInventoryDescription itemDescription;

    List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();

    private int currentlyDraggedItemIndex = -1;

    public event Action<int>
        OnDescriptionRequested,
        OnItemActionRequested,
        OnStartDragging;
    public event Action<int, int>
        OnSwapItems;

    private void Awake()
    {
        //mouseFollower.Toggle(false)
        
    }
    public void InitializeInventoryUI(int inventorySize)
    {
        Debug.Log("inventory initialized");
        for (int i = 0; i < inventorySize; i++)
        {
            UIInventoryItem UIItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            UIItem.transform.SetParent(contentPanel);
            listOfUIItems.Add(UIItem);

            UIItem.OnItemClicked += HandleItemSelection;
            UIItem.OnItemBeginDrag += HandleBeginDrag;
            UIItem.OnItemDroppedOn += HandleSwap;
            UIItem.OnItemEndDrag += HandleEndDrag;
            UIItem.OnItemRMBClicked += HandleShowItemActions;
        }
    }

    private void HandleItemSelection(UIInventoryItem inventoryItemUI)
    {
        int index = listOfUIItems.IndexOf(inventoryItemUI);
        if (index == -1)
            return;
        OnDescriptionRequested?.Invoke(index);
    }
    private void HandleSwap(UIInventoryItem inventoryItemUI)
    {
        int index = listOfUIItems.IndexOf(inventoryItemUI);
        if (index == -1)
        {
            return;
        }
        OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
    }
    private void ResetDraggedItem()
    {
        InventoryController.Instance.mouseFollower.Toggle(false);
        currentlyDraggedItemIndex = -1;
    }
    private void HandleBeginDrag(UIInventoryItem inventoryItemUI)
    {
        int index = listOfUIItems.IndexOf(inventoryItemUI);
        if (index == -1)
            return;
        currentlyDraggedItemIndex = index;
        HandleItemSelection(inventoryItemUI);
        OnStartDragging?.Invoke(index);
    }
    private void CreateDraggedItem(Sprite sprite, int quantity)
    {
        InventoryController.Instance.mouseFollower.Toggle(true);
        InventoryController.Instance.mouseFollower.SetData(sprite, quantity);
    }
    private void HandleEndDrag(UIInventoryItem inventoryItemUI)
    {
        InventoryController.Instance.mouseFollower.Toggle(false);
    }
    public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
    {
        if(listOfUIItems.Count>itemIndex)
        {
            listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
        }
    }
    private void HandleShowItemActions(UIInventoryItem inventoryItemUI)
    {
    }
    public void Show()
    {
        gameObject.SetActive(true);
        ResetSelection();
    }
    private void ResetSelection()
    {
        itemDescription.ResetDescription();
        DeselectAllItems();
    }
    private void DeselectAllItems()
    {
        foreach(UIInventoryItem item in listOfUIItems)
        {
            item.Deselect();
        }
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        itemDescription.ResetDescription();
        ResetDraggedItem();
    }
}
