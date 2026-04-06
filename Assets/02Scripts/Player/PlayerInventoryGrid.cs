using System;
using UnityEngine;
using static UnityEditor.Progress;

public struct GridData
{
    public string ItemID;
    public int Count;
    public int MaxCount;
}

public class PlayerInventoryGrid : MonoBehaviour
{
    GridData[,] inventoryGrid = new GridData[0, 0];
    public GridData[,] InventoryGrid => inventoryGrid;
    public event Action OnSlotChangedAction;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private PlayerEquipment equipment;

    private void Awake()
    {
        if(playerInventory == null)
            playerInventory = GetComponent<PlayerInventory>();

        if(equipment == null)
            equipment = GetComponent<PlayerEquipment>();
    }
    public void Initialzed(int row, int col)
    {
        inventoryGrid = new GridData[row, col];
    }
    //인벤토리 안에서 슬롯 바꿀때
    public void ChangeSlot(int dragRow, int dragCol, int dropRow, int dropCol)
    {
        if(dragRow < 0 || dragCol < 0 || dragRow > inventoryGrid.GetLength(0) - 1 || dragCol > inventoryGrid.GetLength(1) - 1) return;
        if (dropCol < 0 || dropRow < 0 || dropRow > inventoryGrid.GetLength(0) - 1 || dropCol > inventoryGrid.GetLength(1) - 1) return;
        GridData gridTargetData = inventoryGrid[dragRow, dragCol];
        GridData gridData = inventoryGrid[dropRow, dropCol];

        inventoryGrid[dragRow, dragCol] = gridData;
        inventoryGrid[dropRow, dropCol] = gridTargetData;

        OnSlotChangedAction?.Invoke();
    }

    //인벤토리에서 드래그로 아이템 장착할때
    public void EquipItemID(int dragRow, int dragCol, string itemID, ItemType currentItemType)
    {
        if (ItemCatalogManager.Instance.TryGetItemData(itemID, out var data))
        {
            if (data.Type != currentItemType) return;
        }
        GridData gridData = inventoryGrid[dragRow,dragCol];
        
        equipment.EquipItem(gridData.ItemID,data.Type ,out string UnEquipItemID);

        if(string.IsNullOrEmpty(UnEquipItemID)) SetGridData(dragRow, dragCol, UnEquipItemID, 0);
        else
        {
            SetGridData(dragRow, dragCol, UnEquipItemID, 1);
        }
        OnSlotChangedAction?.Invoke();
    }
    //장비에서 드래그로 아이템 장착해제 또는 변경
    public void EquipItemID(string dragItemID,int dropRow, int DropCol)
    {
        ItemCatalogManager.Instance.TryGetItemData(dragItemID, out var data);

        GridData gridData = inventoryGrid[dropRow,DropCol];
        SetGridData(dropRow, DropCol, dragItemID, 1);

        equipment.EquipItem(gridData.ItemID,data.Type ,out string _);
        OnSlotChangedAction?.Invoke();
    }
    //아이템 삭제
    public void SetRemoveItemGrid(string itemId,int amount,int row, int col)
    {
        if(string.IsNullOrEmpty(itemId)) return;
        if(amount <= 0) return;
        if(row < 0 || row > inventoryGrid.GetLength(0) - 1) return;
        if(col < 0 || col > inventoryGrid.GetLength(1) - 1) return;

        if (inventoryGrid[row, col].ItemID != itemId) return;
        inventoryGrid[row, col].Count -= amount;
        if(inventoryGrid[row, col].Count <= 0)
        {
            inventoryGrid[row, col] = new GridData();
        }
    }
    private void SetGridData(int row, int col, string itemID, int amount)
    {
        GridData gridData = new GridData();
        if(ItemCatalogManager.Instance.TryGetItemData(itemID, out var data))
        {
            gridData.ItemID = data.ItemID;
            gridData.Count = amount;
            gridData.MaxCount = data.MaxStack;
        }
        inventoryGrid[row, col] = gridData;
    }
    public int SetAddGrid(string itemID, int amount)
    {
        if (!ItemCatalogManager.Instance.TryGetItemData(itemID, out ItemData itemData))
            return amount;

        for (int col = 0; col < inventoryGrid.GetLength(1); col++)
        {
            for (int row = 0; row < inventoryGrid.GetLength(0); row++)
            {
                GridData gridData = inventoryGrid[row, col];

                if (gridData.ItemID != itemID) continue;
                if (gridData.Count >= gridData.MaxCount) continue;

                int canAdd = gridData.MaxCount - gridData.Count;
                int addAmount = Mathf.Min(amount, canAdd);

                gridData.Count += addAmount;
                amount -= addAmount;

                inventoryGrid[row, col] = gridData;

                if (amount <= 0)
                    return 0;
            }
        }

        for (int col = 0; col < inventoryGrid.GetLength(1); col++)
        {
            for (int row = 0; row < inventoryGrid.GetLength(0); row++)
            {
                GridData gridData = inventoryGrid[row, col];

                if (!string.IsNullOrEmpty(gridData.ItemID)) continue;

                gridData.ItemID = itemID;
                gridData.MaxCount = itemData.MaxStack;

                int addAmount = Mathf.Min(amount, gridData.MaxCount);
                gridData.Count = addAmount;

                inventoryGrid[row, col] = gridData;
                amount -= addAmount;

                if (amount <= 0)
                    return 0;
            }
        }
        if (amount > 0)
        {
            Debug.Log($"인벤토리가 가득 차서 {amount}개를 넣지 못했습니다.");
        }

        return amount;
    }

    [ContextMenu("PrintItemGrid")]
    private void PrintItemGrid()
    {
        string itemDebug = string.Empty;
        Debug.Log("=============Inventory=============");
        for (int col = 0; col < inventoryGrid.GetLength(1); col++)
        {
            for (int row = 0; row < inventoryGrid.GetLength(0); row++)
            {
                itemDebug += $"[{inventoryGrid[row, col].ItemID}]\t";
            }
            Debug.Log(itemDebug);
            itemDebug = string.Empty;
        }
        Debug.Log("====================================");
    }
}