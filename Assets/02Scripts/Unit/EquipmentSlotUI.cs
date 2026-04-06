using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour
{

    [SerializeField] private ItemType equipType;
    [SerializeField] Sprite noneEquipSlotSprite;
    [SerializeField] Sprite equipSlotSprite;
    [SerializeField] Image slotImage;

    [SerializeField] Image iconImage;
    [SerializeField] string currentitemID;
    
    public ItemType EquipType => equipType;
    public string CurrentItemID => currentitemID;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        slotImage.sprite = noneEquipSlotSprite;
        iconImage.sprite = null;
        iconImage.gameObject.SetActive(false);
    }
    public void UnEquip()
    {
        slotImage.sprite = noneEquipSlotSprite;
        iconImage.sprite = null;
        iconImage.gameObject.SetActive(false);
        currentitemID = null;
    }
    public void DrawSlot(string itemID,out string currentitemID)
    {
        Debug.Log(itemID + "°¡ µé¾î¿È");
        currentitemID = null;
        string equipItemID = itemID;

        Initialize();
        
        if (!ItemCatalogManager.Instance.TryGetItemData(equipItemID, out ItemData itemData))
        {
            return;
        }
        currentitemID = this.currentitemID;
        this.currentitemID = equipItemID;
        slotImage.sprite = equipSlotSprite;
        iconImage.sprite = itemData.ItemIcon;
        iconImage.gameObject.SetActive(true);
    }
}
