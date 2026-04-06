using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] ItemData helmetData;
    [SerializeField] ItemData bodyData;
    [SerializeField] ItemData shoesData;
    [SerializeField] ItemData weaponData;

    [SerializeField] EquipmentSlotUI helmetSlot;
    [SerializeField] EquipmentSlotUI bodySlot;
    [SerializeField] EquipmentSlotUI shoesSlot;
    [SerializeField] EquipmentSlotUI weaponSlot;

    private void Awake()
    {
        Initialized();
    }

    private void Initialized()
    {
        if (helmetSlot == null) Debug.LogWarning("헬멧슬롯과 연결되어있지 않습니다");
        if (bodySlot == null) Debug.LogWarning("바디슬롯과 연결되어있지 않습니다");
        if (shoesSlot == null) Debug.LogWarning("신발슬롯과 연결되어있지 않습니다");
        if (weaponSlot == null) Debug.LogWarning("무기슬롯과 연결되어있지 않습니다");
        
        helmetData = new ItemData();
        bodyData = new ItemData();
        shoesData = new ItemData();
        weaponData = new ItemData();
    }

    public void UnEquipItem(string itemID, EquipmentSlotUI slot)
    {
        slot.UnEquip();
    }
    public void EquipItem(string itemID, out string outItemID)
    {
        outItemID = null;

        ItemCatalogManager.Instance.TryGetItemData(itemID, out ItemData itemData);
        Debug.Log(itemID + "교환 시도");
        switch (DragAndDropManager.Instance.CurrentSlotType)
        {
            case ItemType.Weapon: weaponData = itemData; weaponSlot.DrawSlot(itemID, out outItemID); break;
            case ItemType.Head: helmetData = itemData; helmetSlot.DrawSlot(itemID, out outItemID); break;
            case ItemType.Body: bodyData = itemData; bodySlot.DrawSlot(itemID, out outItemID); break;
            case ItemType.Shoes: shoesData = itemData; shoesSlot.DrawSlot(itemID, out outItemID); break;
            default: break;
        }
    }
}
