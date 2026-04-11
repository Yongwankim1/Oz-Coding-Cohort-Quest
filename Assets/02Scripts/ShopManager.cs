using UnityEngine;

public class ShopManager : MonoBehaviour,IInteractable
{
    [SerializeField] ShopGUI shopGUI;
    [SerializeField] PlayerInteract player;
    [SerializeField] PlayerInventory inventory;
    [SerializeField] ShopItemTable itemTable;
    [SerializeField] float orderCoolDown = 0.8f;
    private float timer;
    private float nextTimer = -999f;
    public void Interact(PlayerInteract player)
    {
        if (player == null) return;
        if (shopGUI == null) return;
        shopGUI.gameObject.SetActive(!shopGUI.gameObject.activeSelf);
        shopGUI.Init(itemTable);
        this.player = player;
        inventory = player.GetComponent<PlayerInventory>();
    }

    public void Buy(string itemID)
    {
        if (timer < nextTimer) return;
        if (player == null) return;
        if (inventory == null) return;
        if(string.IsNullOrEmpty(itemID)) return;
        if (!ItemCatalogManager.Instance.TryGetItemData(itemID, out var itemData)) return;

        if (inventory.Gold - itemData.ItemPrice < 0) return;
        else
        {
            if (!inventory.TrySpendGold(itemData.ItemPrice)) return;

            if(inventory.AddItem(itemData.ItemID, 1, out _))
            {
                nextTimer = Time.time + orderCoolDown;

                Debug.Log("구매성공");
            }
            else
            {
                inventory.AddGold(itemData.ItemPrice);
                Debug.Log("구매 실패");
            }
        }
    }

    private void Awake()
    {
        if(shopGUI == null)
        {
            Debug.LogWarning("shopGUI 참조 안됨");
            shopGUI = GameObject.Find("ShopPanel").GetComponent<ShopGUI>();
        }
        if(itemTable == null)
        {
            Debug.LogWarning("ShopItemTable 참조 안됨");
            itemTable = GetComponent<ShopItemTable>();
        }
    }

    void Update()
    {
        timer = Time.time;
    }
}
