using UnityEngine;

public enum ItemType
{
    None,

    Weapon,
    Head,
    Body,
    Shoes,

    HPPotion,

    Material,
    Quest
}

[System.Serializable]
public struct ItemData
{
    public ItemType Type;
    public string ItemID;
    public string DisplayName;
    public string Description;
    public int MaxStack;
    public Sprite ItemIcon;
    public int Value;
    public int ItemPrice;
}
public abstract class ItemObject : ScriptableObject
{
    [SerializeField] ItemData itemData;
    public ItemData ItemData
    { 
        get { return itemData; }
        private set { itemData = value; }
    }

    public abstract void Use(PlayerInventoryGrid inventory, InventorySlotUI slotUI);

}

