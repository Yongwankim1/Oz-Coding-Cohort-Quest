using System.Collections.Generic;
using UnityEngine;

public class ItemCatalogManager : MonoBehaviour
{
    public static ItemCatalogManager Instance;

    [SerializeField] List<ItemObject> registeredItems = new List<ItemObject> ();
    //등록시킬 ItemID와 ItemData
    private Dictionary<string, ItemData> registeredItemData = new Dictionary<string, ItemData> ();
    private Dictionary<string, ItemObject> registeredItemClass = new Dictionary<string, ItemObject> ();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void Start()
    {
        Initialized();
    }
    private void Initialized()
    {
        registeredItemData.Clear();
        foreach (ItemObject item in registeredItems)
        {
            registeredItemData.Add(item.ItemData.ItemID, item.ItemData);
            registeredItemClass.Add(item.ItemData.ItemID, item);
        }
    }

    private bool IsRegisteredItem(string itemID)
    {
        if(string.IsNullOrEmpty(itemID)) return false;
        if (registeredItemData.ContainsKey(itemID.Trim()))
        {
            return true;
        }
        Debug.LogWarning("등록되지 않은 아이템스크립터블입니다");
        return false;

    }

    public ItemType GetItemType(string itemID)
    {
        if (!IsRegisteredItem(itemID))
        {
            return ItemType.None;
        }
        
        return registeredItemData[itemID].Type;
    }
    public bool TryGetItemData(string itemID, out ItemData itemData)
    {
        itemData = default;
        if(string.IsNullOrWhiteSpace(itemID)) return false;
        if(!IsRegisteredItem(itemID)) return false;

        if (registeredItemData.TryGetValue(itemID, out itemData))
            return true;

        return false;
    }

    public bool TryGetItemClass(string itemID, out ItemObject itemClass)
    {
        itemClass = default;
        if (string.IsNullOrWhiteSpace(itemID)) return false;
        if(registeredItemClass.TryGetValue(itemID, out itemClass))
        {
            return true;
        }
        return false;
    }
    public int GetMaxStack(string itemID)
    {
        TryGetItemData(itemID, out ItemData itemData);
        return itemData.MaxStack;
    }
}
