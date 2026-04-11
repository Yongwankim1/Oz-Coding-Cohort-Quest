
using System.Collections.Generic;
using UnityEngine;

public class ShopItemTable : MonoBehaviour
{
    [SerializeField] private List<ItemObject> itemList;

    [SerializeField] private Dictionary<string,ItemObject> equipList = new Dictionary<string, ItemObject>();
    [SerializeField] private Dictionary<string,ItemObject> consumeList = new Dictionary<string, ItemObject>();
    [SerializeField] private Dictionary<string,ItemObject> materialList = new Dictionary<string, ItemObject>();

    public IReadOnlyDictionary <string, ItemObject> EquipList => equipList;
    public IReadOnlyDictionary <string, ItemObject> ConsumeList => consumeList;
    public IReadOnlyDictionary <string, ItemObject> MaterialList => materialList;

    public int Count => itemList.Count;
    void Start()
    {
        Initialize();
    }
    
    void Initialize()
    {
        equipList.Clear();
        consumeList.Clear();
        materialList.Clear();
        if (itemList.Count <= 0) return;

        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i] == null) continue;
            ItemType type = itemList[i].ItemData.Type;
            switch (type)
            {
                case ItemType.None: continue;
                case ItemType.Weapon:
                case ItemType.Head:
                case ItemType.Body: 
                case ItemType.Shoes: ListAdd(itemList[i], equipList); break;
                case ItemType.HPPotion: ListAdd(itemList[i], consumeList); break;
                case ItemType.Material:
                case ItemType.Quest: ListAdd(itemList[i], materialList); break;
            }
        }
    }


    private void ListAdd(ItemObject item, Dictionary<string, ItemObject> list)
    {
        if (!list.ContainsKey(item.ItemData.ItemID)) list.Add(item.ItemData.ItemID, item);
        else
        {
            list[item.ItemData.ItemID] = item;
        }
        return;
    }
}
