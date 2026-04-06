using UnityEngine;
public enum DropType
{
    None,
    Equip,
    Inventory
}
public class DragAndDropManager : MonoBehaviour
{
    public static DragAndDropManager Instance;

    public DropType DragType = DropType.None;
    public DropType DropType = DropType.None;


    public Vector2 DragingSlot = new Vector2(-1,-1);
    public Vector2 DropSlot = new Vector2(-1,-1);

    public ItemType CurrentSlotType = ItemType.None;
    //public GridData DragData = new GridData();
    //public string DragItemID = string.Empty;

    [SerializeField] PlayerInventoryGrid inventoryGrid;
    //[SerializeField] PlayerEquipment playerEquipment;
    //[SerializeField] PlayerInventory playerInventory;
    public string dragingItemID;

    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //if(inventoryGrid == null) inventoryGrid = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventoryGrid>();
    }

    public void ItemSlotChanged()
    {
        //인벤토리에서 인벤토리
        if (DragType == DropType.Inventory && DropType == DropType.Inventory)
        {
            inventoryGrid.ChangeSlot((int)DragingSlot.x, (int)DragingSlot.y, (int)DropSlot.x, (int)DropSlot.y);
        }
        //인벤토리에서 장비
        else if (DragType == DropType.Inventory && DropType == DropType.Equip)
        {
            //인벤토리에서 장착 후 인벤토리에서 삭제 장착되어 있던 장비 빠짐 빠진 장비 그리드에 다시 추가
            inventoryGrid.EquipItemID((int)DragingSlot.x, (int)DragingSlot.y, dragingItemID, CurrentSlotType);
        }
        else if(DragType == DropType.Equip && DropType == DropType.Inventory)
        {
            //장비창에서 인벤토리로 장착 해제 후 슬롯에 장비가 있으면 장비를 인벤토리에 장착
            inventoryGrid.EquipItemID(dragingItemID,(int)DropSlot.x,(int)DropSlot.y);
        }
    }



    void Initialize()
    {
        DragingSlot = new Vector2(-1,-1);
        DropSlot = new Vector2(-1,-1);
        dragingItemID = string.Empty;
    }
    //public void ItemSlotChanged()
    //{
    //    if (Type == DropType.Inventory)
    //    {
    //        if(DragingSlot.x >= 0 && DragingSlot.y >= 0 && DropSlot.x >= 0 && DropSlot.y >= 0 &&
    //            DragingSlot.x < playerInventory.RowCount && DragingSlot.y < playerInventory.ColumnCount 
    //            && DropSlot.x < playerInventory.RowCount && DropSlot.y < playerInventory.ColumnCount)
    //        {
    //            inventoryGrid.ChangeSlotItemId((int)DragingSlot.x, (int)DragingSlot.y, (int)DropSlot.x, (int)DropSlot.y);
    //        }
    //        ///TODO:: 장비창에서 빼서 아이템에 슬롯에 넣어주는 코드도 추가해줘야함
    //        if(!string.IsNullOrWhiteSpace(DragItemID))
    //        {
    //            inventoryGrid.ChangeSlotItemId(DragItemID, 1,(int)DropSlot.x, (int)DropSlot.y, out string itemID);
    //            playerEquipment.UnEquipItem(itemID, CurrentEquipSlot);
    //        }
    //    }
    //    else if (Type == DropType.Equip)
    //    {
    //        ItemCatalogManager.Instance.TryGetItemData(DragData.ItemID, out ItemData itemData);
    //        if (CurrentSlotType != itemData.Type)
    //        {
    //            Initialize();
    //            return;
    //        }
    //        playerEquipment.EquipItem(DragData.ItemID, out string outItemID);
    //        playerInventory.RemoveItem(DragData.ItemID, DragData.Count, (int)DragingSlot.x, (int)DragingSlot.y);
    //        if (!string.IsNullOrEmpty(outItemID))
    //        {
    //            playerInventory.AddItem(outItemID, 1, out _);
    //        }
    //    }
    //    Initialize();
    //}
}
// 인벤토리에서 아이템드래그 시작(데이터 담음) -> 장비칸에 드랍(드랍할 데이터 담음) -> 장비칸 기존 아이템 자리 바꿈

// 장비칸에서 아이템 드래그 시작 -> 인벤토리 빈칸 or 장비가 있는 슬롯에 드랍 -> 자리 바꿈