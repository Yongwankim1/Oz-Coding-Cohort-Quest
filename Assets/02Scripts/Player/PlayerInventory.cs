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
    [Header("inventoryUI")]
    [SerializeField] private GameObject inventoryPanel;
    [Header("EquipmentPanel")]
    [SerializeField] private GameObject equipmentPanel;

    [SerializeField] private int gold;
    public int Gold { get { return gold; } set { gold = value; } }

    private void Awake()
    {
        Init();
    }
    private void Update()
    {
        if (inputReader.IsInventoryTogglePerformedThisFrame)
        {
            InventoryToggle();
        }
    }
    private void InventoryToggle()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        equipmentPanel.SetActive(!equipmentPanel.activeSelf);
        inputReader.CanMove = !inventoryPanel.activeSelf;

        if (!inventoryPanel.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            OnItemAmountChanged();
        }
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

    private void IncreaseItemCount(string itemId, int amount)
    {
        if (amount <= 0) return;

        if (itemIdByCount.TryGetValue(itemId, out int value))
            itemIdByCount[itemId] = value + amount;
    }

    private void DecreaseItemCount(string itemId, int amount)
    {
        if (string.IsNullOrWhiteSpace(itemId)) return;
        if (amount <= 0) return;

        if (!itemIdByCount.TryGetValue(itemId, out int currentValue))
        {
            return;
        }

        int newValue = currentValue - amount;

        if (newValue <= 0)
        {
            itemIdByCount.Remove(itemId);
        }
        else
        {
            itemIdByCount[itemId] = newValue;
        }
    }

    public bool TrySpendGold(int amount)
    {
        if(amount < 0) return false;
        if(gold - amount < 0) return false;

        gold -= amount;
        OnItemAmountChanged?.Invoke();
        return true;
    }
    public void AddGold(int amount)
    {
        if(amount <= 0) return;

        gold += amount; 
        OnItemAmountChanged?.Invoke();
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