using UnityEngine;

public class WorldItem : MonoBehaviour, IInteractable
{
    [SerializeField] ItemObject itemObject;
    [SerializeField] int itemAmount;
    public void Interact(PlayerInteract player)
    {
        PickUP(player);
    }
    void PickUP(PlayerInteract player)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        if (inventory == null) return;

        if (inventory.AddItem(itemObject.ItemData.ItemID, itemAmount, out int restAmount))
        {
            itemAmount = restAmount;
            gameObject.SetActive(restAmount > 0);
        }
        player.RemoveInteract(this);
    }
}
