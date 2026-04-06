using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform canvas;
    Transform previousParent;
    RectTransform rect;
    CanvasGroup canvasGroup;

    [SerializeField] InventorySlotUI inventorySlotUI;
    void Awake()
    {
        Initialize();
    }
    void Initialize()
    {
        inventorySlotUI = GetComponentInParent<InventorySlotUI>();
        canvas = GetComponentInParent<Canvas>().transform;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        DragAndDropManager.Instance.DragingSlot.x = inventorySlotUI.Row;
        DragAndDropManager.Instance.DragingSlot.y = inventorySlotUI.Col;
        DragAndDropManager.Instance.DragData = inventorySlotUI.GridData;

        previousParent = transform.parent;

        transform.SetParent(canvas);
        transform.SetAsLastSibling();

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(previousParent);
        rect.position = previousParent.GetComponent<RectTransform>().position;

        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
        DragAndDropManager.Instance.ItemSlotChanged();
    }
}
