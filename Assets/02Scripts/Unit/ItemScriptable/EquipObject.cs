using UnityEngine;

[CreateAssetMenu(menuName = "Item/Equip Item")]
public class EquipObject : ItemObject
{
    public override void Use(PlayerInventoryGrid inventory, InventorySlotUI slotUI)
    {
        DragAndDropManager.Instance.DragType = DropType.Inventory;
        DragAndDropManager.Instance.DropType = DragAndDropManager.Instance.GetDropType(ItemData.Type);
        DragAndDropManager.Instance.DragingSlot = new Vector2(slotUI.Row, slotUI.Col);
        DragAndDropManager.Instance.dragingItemID = slotUI.ItemID;
        DragAndDropManager.Instance.CurrentSlotType = ItemCatalogManager.Instance.GetItemType(slotUI.ItemID);
        DragAndDropManager.Instance.ItemSlotChanged();
    }
}
