using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    [Tooltip("플레이어 가방에 어느 위치에 있는지 그리드로 나타내주는 클래스")]
    [SerializeField] PlayerInventoryGrid inventoryGrid;

    [Tooltip("플레이어가 최대 소지할 수 있는 아이템 칸입니다")]
    [SerializeField] int rowCount; //행 X ->->->
    [SerializeField] int columnCount;//열 Y ^^^^

    //플레이어가 가지고 있는 아이템
    Dictionary<string, int> itemIdByCount = new Dictionary<string, int>();
    Stack<ItemData> undoItemData = new Stack<ItemData>();
    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        if(inventoryGrid == null)
            inventoryGrid = GetComponent<PlayerInventoryGrid>();
        inventoryGrid.Init(rowCount, columnCount);
    }

    public bool AddItem(string itemID, int amount)
    {
        if (string.IsNullOrWhiteSpace(itemID)) return false;
        if (amount <= 0) return false;
        if (!ItemCatalogManager.Instance.TryGetItemData(itemID, out _)) return false;
        if (!itemIdByCount.ContainsKey(itemID))
        {
            itemIdByCount.Add(itemID, amount);
            return true;
        }
        else
        {
            IncreaseItemCount(itemID, amount);
            return false;
        }
    }
    
    private void IncreaseItemCount(string itemId, int amount)
    {
        if(amount <= 0) return;
        if (itemIdByCount.TryGetValue(itemId, out int value))
        {
            itemIdByCount[itemId] = value + amount;
        }
    }
    private void DecreaseItemCount(string itemId, int amount)
    {
        if (amount <= 0) return;
        if (itemIdByCount.TryGetValue(itemId, out int value))
        {
            itemIdByCount[itemId] = value - amount;
        }
    }


}
