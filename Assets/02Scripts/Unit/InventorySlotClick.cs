using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] InventorySlotUI inventorySlotUI;

    void Awake()
    {
        if(inventorySlotUI == null)
            inventorySlotUI = GetComponent<InventorySlotUI>();

    }
    private DropType GetDropType()
    {
        ItemType itemType = ItemCatalogManager.Instance.GetItemType(inventorySlotUI.ItemID);
        switch (itemType)
        {
            case ItemType.Body: return DropType.Equip;
            case ItemType.Head: return DropType.Equip;
            case ItemType.Shoes: return DropType.Equip;
            case ItemType.Weapon: return DropType.Equip;
            default: return DropType.None;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            ItemType itemType = ItemCatalogManager.Instance.GetItemType(inventorySlotUI.ItemID);
            if (itemType == ItemType.HPPotion)
            {
                ///TODO:: 포션 사용 메서드 구현
                return;
            }
            Debug.Log("우클릭 감지");
            if (inventorySlotUI.ItemID == null) return;
            DragAndDropManager.Instance.DragType = DropType.Inventory;
            DragAndDropManager.Instance.DropType = GetDropType();
            DragAndDropManager.Instance.DragingSlot = new Vector2(inventorySlotUI.Row,inventorySlotUI.Col);
            DragAndDropManager.Instance.dragingItemID = inventorySlotUI.ItemID;
            DragAndDropManager.Instance.CurrentSlotType = ItemCatalogManager.Instance.GetItemType(inventorySlotUI.ItemID);
            DragAndDropManager.Instance.ItemSlotChanged();
        }
        else if(eventData.button == PointerEventData.InputButton.Left)
        {

            Debug.Log("좌클릭 감지");
        }
    }


}
