using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] PlayerInputReader inputReader;
    [SerializeField] PlayerInventoryGrid inventoryGrid;

    [Header("창고 개수")]
    [SerializeField] int rowCount;
    [SerializeField] int columnCount;

    public int RowCount => rowCount;
    public int ColumnCount => columnCount;
    public PlayerInventoryGrid InventoryGrid => inventoryGrid;

    Dictionary<string, int> itemIdByCount = new Dictionary<string, int>();
    public Dictionary<string, int> ItemIdByCount => itemIdByCount;

    public event Action OnItemAmountChanged;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (inputReader == null) inputReader = GetComponent<PlayerInputReader>();

        if (inventoryGrid == null) inventoryGrid = GetComponent<PlayerInventoryGrid>();

        if (inventoryGrid != null) inventoryGrid.Initialzed(RowCount, ColumnCount);

        itemIdByCount.Clear();
    }

    private void RaiseItemChanged() => OnItemAmountChanged?.Invoke();
    public void AddItem(string itemId, int amount)
    {
        if (!itemIdByCount.ContainsKey(itemId))
            itemIdByCount.Add(itemId, amount);
        else
            IncreaseItemCount(itemId, amount);
    }
    public bool AddItem(string itemID, int amount, out int restAmount)
    {
        restAmount = -1;

        if (string.IsNullOrWhiteSpace(itemID)) return false;
        if (amount <= 0) return false;
        if (!ItemCatalogManager.Instance.TryGetItemData(itemID, out _)) return false;
        if (inventoryGrid == null) return false;

        restAmount = inventoryGrid.SetAddGrid(itemID, amount);
        int addedAmount = amount - restAmount;

        if (addedAmount <= 0)
            return true;

        if (!itemIdByCount.ContainsKey(itemID))
            itemIdByCount.Add(itemID, addedAmount);
        else
            IncreaseItemCount(itemID, addedAmount);

        RaiseItemChanged();
        return true;
    }
    public void RemoveItem(string ItemID, int amount)
    {
        DecreaseItemCount(ItemID, amount);
    }
    public bool RemoveItem(string itemID, int amount,int row, int col)
    {
        if (amount <= 0) return false;
        if(string.IsNullOrWhiteSpace(itemID)) return false;

        if (!itemIdByCount.ContainsKey(itemID))
        {
            return false;
        }
        else
        {
            DecreaseItemCount(itemID, amount);
        }
        inventoryGrid.SetRemoveItemGrid(itemID,amount,row,col);
        RaiseItemChanged();
        return true;

    }
    private void IncreaseItemCount(string itemId, int amount)
    {
        if (amount <= 0) return;

        if (itemIdByCount.TryGetValue(itemId, out int value))
            itemIdByCount[itemId] = value + amount;
    }

    private void DecreaseItemCount(string itemId, int amount)
    {
        if (amount <= 0) return;

        if (itemIdByCount.TryGetValue(itemId, out int value))
        {
            int newValue = value - amount;

            if (newValue <= 0)
                itemIdByCount.Remove(itemId);
            else
                itemIdByCount[itemId] = newValue;
        }
    }
    [ContextMenu("printItem")]
    private void PrintItemIdByCount()
    {
        if(itemIdByCount.Count == 0)
        {
            Debug.Log("아이템이 없습니다");
            return;
        }
        foreach(var item in itemIdByCount)
        {
            Debug.Log(item.Key +"가 " + item.Value +"있습니다");
        }
    }
}