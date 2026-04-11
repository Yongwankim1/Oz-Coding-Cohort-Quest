using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] InventorySlotUI inventorySlotUI;
    [SerializeField] PlayerHP playerHP;
    [SerializeField] PlayerInventoryGrid playerInventoryGrid;
    void Awake()
    {
        if(inventorySlotUI == null)
            inventorySlotUI = GetComponent<InventorySlotUI>();
        if (playerInventoryGrid == null)
            playerInventoryGrid = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventoryGrid>();
        if (playerHP == null) playerHP = playerInventoryGrid.GetComponent<PlayerHP>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (inventorySlotUI.ItemID == null) return;
            //if(ItemCatalogManager.Instance.TryGetItemData(inventorySlotUI.ItemID, out ItemData itemData))
            if (!ItemCatalogManager.Instance.TryGetItemClass(inventorySlotUI.ItemID, out var itemClass)) return;
            itemClass.Use(playerInventoryGrid, inventorySlotUI);
            return;

        }
        else if(eventData.button == PointerEventData.InputButton.Left)
        {

            Debug.Log("좌클릭 감지");
        }
    }
    private bool UsePotion(string itemID)
    {
        if(!ItemCatalogManager.Instance.TryGetItemData(itemID, out var data))
        {
            return false;
        }
        if(playerInventoryGrid == null)
            playerInventoryGrid = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventoryGrid>();
        if (playerHP == null) playerHP = playerInventoryGrid.GetComponent<PlayerHP>();

        if (playerHP.CurrentHP == playerHP.MaxHP)
        {
            Debug.Log("체력이 가득 차 있습니다");
            return false;
        }
        playerInventoryGrid.RemoveItem(inventorySlotUI.Row, inventorySlotUI.Col, 1);
        playerHP.Heal(data.Value);
        return true;
    }

}
