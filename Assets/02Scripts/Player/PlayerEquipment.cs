using NUnit.Framework.Interfaces;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] EquipmentSlotUI helmetSlot;
    [SerializeField] EquipmentSlotUI bodySlot;
    [SerializeField] EquipmentSlotUI shoesSlot;
    [SerializeField] EquipmentSlotUI weaponSlot;

    [SerializeField] PlayerAttack playerAttack;
    [SerializeField] PlayerHP playerHP;
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
    }

    public void EquipItem(string itemID, ItemType itemType, out string UnEquipItemID)
    {
        if (itemType == ItemType.Weapon)
        {
            weaponSlot.EquipItem(itemID,out UnEquipItemID);
        }
        else if (itemType == ItemType.Body)
        {
            bodySlot.EquipItem(itemID, out UnEquipItemID);
        }
        else if (itemType == ItemType.Head)
        {
            helmetSlot.EquipItem(itemID, out UnEquipItemID);
        }
        else if (itemType == ItemType.Shoes)
        {
            shoesSlot.EquipItem(itemID, out UnEquipItemID);
        }
        else
        {
            UnEquipItemID = null;
        }
        float weaponValue = weaponSlot.ItemValue;
        float hpValue = helmetSlot.ItemValue + bodySlot.ItemValue + shoesSlot.ItemValue;
        playerAttack.EquipWeapon(weaponValue);
        playerHP.EquipItem((int) hpValue);
    }
}
