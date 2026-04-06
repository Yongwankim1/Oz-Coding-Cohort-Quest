using UnityEngine;
using UnityEngine.EventSystems;

public class EquipSlotClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] EquipmentSlotUI slotUI;

    void Awake()
    {
        if(slotUI == null) slotUI = GetComponent<EquipmentSlotUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("클릭감지");
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("우클릭 감지");
            if (slotUI.CurrentItemID == null) return;
            DragAndDropManager.Instance.DragType = DropType.Equip;
            DragAndDropManager.Instance.DropType = DropType.Equip;
            DragAndDropManager.Instance.dragingItemID = slotUI.CurrentItemID;
            DragAndDropManager.Instance.ItemSlotChanged();
            slotUI.EquipItem(null, out _);
        }
    }
}
