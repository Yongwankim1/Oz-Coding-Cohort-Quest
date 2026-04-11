using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] Image iconImage;

    [SerializeField] string itemID;
    [SerializeField] ShopGUI shopGUI;
    public string ItemID => itemID;

    public void Initialize()
    {
        itemID = string.Empty;
        gameObject.SetActive(false);
    }
    public void Initialize(string itemID,ShopGUI shopGUI)
    {
        if (string.IsNullOrEmpty(itemID)) return;
        if (!ItemCatalogManager.Instance.TryGetItemData(itemID, out var data))
        {
            return;
        }
        this.shopGUI = shopGUI;
        this.itemID = data.ItemID;
        iconImage.sprite = data.ItemIcon;
        iconImage.gameObject.SetActive(data.ItemID != null);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            shopGUI.SetSelectSlot(this);
        }
    }


}
