using UnityEngine;
[CreateAssetMenu(menuName = "Item/HP Potion")]
public class PotionObject : ItemObject
{
    public override void Use(PlayerInventoryGrid inventory, InventorySlotUI slotUI)
    {
        if (inventory == null || slotUI == null) return;

        PlayerHP playerHP = inventory.GetComponent<PlayerHP>();
        if (playerHP == null) return;
        if (playerHP.CurrentHP >= playerHP.MaxHP) return;

        inventory.RemoveItem(slotUI.Row, slotUI.Col, 1);
        playerHP.Heal(ItemData.Value);
    }

}
