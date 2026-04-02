using UnityEngine;

public enum ItemType
{
    None,

    Weapon,
    Head,
    Body,
    Shoes,

    Consume,

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
}
[CreateAssetMenu(fileName = "New Item",menuName = "Item/Item Object")]
public class ItemObject : ScriptableObject
{
    [SerializeField] ItemData itemData;
    public ItemData ItemData
    { 
        get { return itemData; }
        private set { itemData = value; }
    }
}

