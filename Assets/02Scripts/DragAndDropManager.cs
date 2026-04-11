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

    [SerializeField] PlayerInventoryGrid inventoryGrid;
    [SerializeField] PlayerInventory playerInventory;
    public string dragingItemID;

    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
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
        else if (DragType == DropType.Equip && DropType == DropType.Inventory)
        {
            //장비창에서 인벤토리로 장착 해제 후 슬롯에 장비가 있으면 장비를 인벤토리에 장착
            inventoryGrid.EquipItemID(dragingItemID, (int)DropSlot.x, (int)DropSlot.y);
            Debug.Log("장비에서 인벤토리");
        }
        else if (DragType == DropType.Equip && DropType == DropType.Equip)
        {
            playerInventory.AddItem(dragingItemID, 1,out int restAmount);
            Debug.Log("장비 우클릭");
        }
        Initialize();
    }



    void Initialize()
    {
        DragingSlot = new Vector2(-1,-1);
        DropSlot = new Vector2(-1,-1);
        dragingItemID = string.Empty;
        CurrentSlotType = 0;
        DragType = 0;
        DropType = 0;
    }
    public DropType GetDropType(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Body: return DropType.Equip;
            case ItemType.Head: return DropType.Equip;
            case ItemType.Shoes: return DropType.Equip;
            case ItemType.Weapon: return DropType.Equip;
            default: return DropType.None;
        }
    }
}
// 인벤토리에서 아이템드래그 시작(데이터 담음) -> 장비칸에 드랍(드랍할 데이터 담음) -> 장비칸 기존 아이템 자리 바꿈

// 장비칸에서 아이템 드래그 시작 -> 인벤토리 빈칸 or 장비가 있는 슬롯에 드랍 -> 자리 바꿈