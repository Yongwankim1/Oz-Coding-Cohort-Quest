using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipDroppableUI : MonoBehaviour, IDropHandler, IPointerExitHandler, IPointerEnterHandler
{
    EquipmentSlotUI equipmentSlotUI;
    Image myImage;
    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        if (equipmentSlotUI == null) equipmentSlotUI = GetComponent<EquipmentSlotUI>();
        myImage = GetComponent<Image>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        DragAndDropManager.Instance.DropType = DropType.Equip;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DragAndDropManager.Instance.CurrentSlotType = equipmentSlotUI.EquipType;
        myImage.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DragAndDropManager.Instance.DropType = DropType.None;
        myImage.color = Color.white;
    }

}
