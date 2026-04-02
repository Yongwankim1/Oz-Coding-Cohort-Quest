using UnityEngine;

public class PlayerInventoryGrid : MonoBehaviour
{
    ItemData[,] inventoryGrid = new ItemData[0,0];

    public void Init(int row, int col)
    {
        inventoryGrid = new ItemData[row, col];
    }
    public void AddItem(int row, int col, ItemData item)
    {
        inventoryGrid[row, col] = item;
    }
    public ItemData RemoveItem(int row, int col)
    {
        ItemData item = inventoryGrid[row, col];
        inventoryGrid[row, col] = default;
        return item;
    }
    public bool HasItem(int row, int col) => HasSlotItem(row, col);
    private bool HasSlotItem(int row, int col)
    {
        return IsVaildPosition(row,col) && inventoryGrid[row,col].Type != ItemType.None;
    }

    private bool IsVaildPosition(int row, int col)
    {
        return row >= 0 && col >= 0 && row < inventoryGrid.GetLength(0) && col < inventoryGrid.GetLength(1);
    }
}
