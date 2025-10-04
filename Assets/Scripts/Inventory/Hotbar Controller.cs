using UnityEngine;
using Inventory.Model;
using Inventory.UI;

public class HotbarController : MonoBehaviour
{
    [SerializeField] private InventorySO inventory;
    [SerializeField] private UIHotbar hotbarUI;
    [SerializeField] private UIInventoryPage inventoryUI;
    private int selectedIndex = 0;

    private void Start()
    {
        hotbarUI.HighlightSlot(selectedIndex);

        GameInput.Instance.OnPlayerAttack += UseSelectedItem;
        GameInput.Instance.OnHotbarScroll += (sender, scroll) =>
        {
            if (scroll > 0f) selectedIndex--;
            else if (scroll < 0f) selectedIndex++;

            if (selectedIndex < 0) selectedIndex = inventory.HotbarSlots.Count - 1;
            if (selectedIndex >= inventory.HotbarSlots.Count) selectedIndex = 0;

            hotbarUI.HighlightSlot(selectedIndex);
        };
    }

    private void UseSelectedItem(object sender, System.EventArgs e)
    {
        if (inventoryUI.gameObject.activeSelf)
            return;
        
        int invIndex = inventory.HotbarSlots[selectedIndex];
        var inventoryItem = inventory.GetItemAt(invIndex);

        if (!inventoryItem.IsEmpty)
        {
            var actionItem = inventoryItem.item as IItemAction;
            if (actionItem != null)
            {
                actionItem.PerformAction(gameObject, inventoryItem.itemState);
                
                if (actionItem.actionSFX != null)
                    AudioSource.PlayClipAtPoint(actionItem.actionSFX, transform.position);
                
                var destroyableItem = inventoryItem.item as IDestroyableItem;
                if (destroyableItem != null)
                {
                    inventory.RemoveItem(invIndex, 1);
                }
            }
        }
    }
}