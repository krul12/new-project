using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;

namespace Inventory.UI
{
    public class UIHotbar : MonoBehaviour
    {
        [SerializeField] private UIInventoryItem itemPrefab;
        [SerializeField] private RectTransform contentPanel;
        [SerializeField] private InventorySO inventoryData;

        public List<UIInventoryItem> uiSlots = new List<UIInventoryItem>();
        
        public bool IsInventoryOpen { get; private set; }
        

        private void Awake()
        {
            
        }

        public void InitializeHotbar(InventorySO inventory)
        {
            Debug.Log("Init hotbar");
            inventoryData = inventory;

            for (int i = 0; i < inventoryData.HotbarSlots.Count; i++)
            {
                var uiItem = Instantiate(itemPrefab, contentPanel);
                uiSlots.Add(uiItem);
                uiItem.Deselect();
            }

            ResetHotbar(); 
            UpdateHotbar();
            inventoryData.OnInventoryUpdated += (_) => UpdateHotbar();
        }

        public void UpdateHotbar()
        {
            if (inventoryData == null || uiSlots.Count == 0)
                return;

            for (int i = 0; i < inventoryData.HotbarSlots.Count; i++)
            {
                var uiSlot = uiSlots[i];
                int invIndex = inventoryData.HotbarSlots[i];
                var item = inventoryData.GetItemAt(invIndex);

                if (!item.IsEmpty)
                    uiSlot.SetData(item.item.ItemImage, item.quantity);
                else
                    uiSlot.ResetData();
            }
        }
        
        

        public void HighlightSlot(int index)
        {
            for (int i = 0; i < uiSlots.Count; i++)
                uiSlots[i].borderImage.enabled = (i == index);
        }

        private void ResetHotbar()
        {
            foreach (var slot in uiSlots)
            {
                if (slot == null) continue;
                slot.ResetData();
                slot.Deselect();
            }
        }
    }
}
