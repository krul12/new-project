using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory.Model // Definiert einen Namensraum für den Inventar-Code
{
    [CreateAssetMenu] // Erlaubt das Erstellen von InventorySO als Asset im Unity Editor
    public class InventorySO : ScriptableObject // Definiert die Inventarklasse, die als Unity-Asset gespeichert wird
    {
        [SerializeField] private List<InventoryItem> inventoryItems; // Liste der Regale/Slots, die Items enthalten
        [field: SerializeField] public int Size { get; private set; } = 24; // Anzahl der Regale/Slots im Inventar, sichtbar im Editor

        
        public event Action<Dictionary<int, InventoryItem>> 
            OnInventoryUpdated;
        // Event, das ausgelöst wird, wenn sich das Inventar ändert

        
        public void Initialize() // Methode zum Vorbereiten des Inventars
        {
            inventoryItems = new List<InventoryItem>(); // Lege eine neue leere Liste von Regalen an
            for (int i = 0; i < Size; i++) // Gehe durch jedes Regal
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem()); // Füge ein leeres Item auf jede Regalposition hinzu
            }
        }

        public int AddItem(ItemSO item, int quantity) // Definiert eine Methode, die Items ins Inventar legt. 'item' = (Apfel, Banane usw.) 'quantity' = wie viele. Rückgabe: Anzahl der Items, die nicht ins Inventar passen
        {
            if(item.IsStackable == false) // Prüft, ob das Item nicht stapelbar ist (jedes Item braucht eine eigene Box)
            {
                for (int i = 0; i < inventoryItems.Count; i++) // Geht durch jedes Regal/Slot im Inventar
                {
                    while (quantity > 0 && IsInventoryFull() == false)   // Solange noch Items übrig sind und das Inventar nicht voll ist
                    {
                        quantity -= AddItemToFirstFreeSlot(item, 1);
                        // Lege eine Einheit auf das erste freie Regal
                        // Ziehe diese Einheit von der Gesamtmenge ab
                    }
                    InformAboutChange(); // Benachrichtigt alle, dass sich das Inventar geändert hat
                    return quantity;  // Gibt zurück, wie viele Items nicht mehr ins Inventar passen
                }
            }
            quantity = AddStackableItem(item, quantity); // Wenn das Item stapelbar ist, versuche, es zu vorhandenen Stapeln hinzuzufügen
            return quantity;  // Gibt die verbleibende Anzahl zurück, die nicht hinzugefügt werden konnte
        }

        private int AddItemToFirstFreeSlot(ItemSO item, int quantity) //Methode: Lege ein Item in den ersten freien Platz im Inventar. Rückgabe: Wie viele Stücke erfolgreich gelegt wurden.
        {
            InventoryItem newItem = new InventoryItem
            {
                item = item,
                quantity = quantity
                // Erstellt ein neues "Paket" = Kombination aus Essen + Anzahl.
                // Beispiel: Apfel, 3 Stück.
            };

            for (int i = 0; i < inventoryItems.Count; i++) // Geht alle Regale/Slots im Inventar durch.
            {
                if (inventoryItems[i].IsEmpty)  // Wenn dieses Regal leer ist ...
                {
                    inventoryItems[i] = newItem; // ... lege das neue Paket (z. B. 3 Äpfel) in dieses Regal.
                    return quantity; // Gib zurück, wie viele Items du erfolgreich gelegt hast.
                }
            }
            return 0; // Wenn KEIN Regal frei war → 0 Items wurden abgelegt.
        }

        private bool IsInventoryFull() => !inventoryItems.Any(item => item.IsEmpty);
        

        private int AddStackableItem(ItemSO item, int quantity) // Methode: Versucht, stapelbare Items (z. B. Zucker, Reis) ins Inventar zu legen. // Rückgabe: Wie viele Stücke NICHT ins Inventar gepasst haben.
        {
            for (int i = 0; i < inventoryItems.Count; i++) // Geht alle Regale im Inventar durch
            {
                if (inventoryItems[i].IsEmpty) continue; // Wenn das Regal leer ist → überspringen (nichts zu tun)
                if (inventoryItems[i].item.ID == item.ID) // Wenn in diesem Regal bereits das gleiche Item liegt (z. B. Zucker zu Zucker)
                {
                    int amountPossibleToTake = inventoryItems[i].item.MaxStackSize - inventoryItems[i].quantity; // Berechnet, wie viele Stücke noch in den Stapel passen. Beispiel: Max 10 Zucker, aktuell 6 drin → 4 passen.

                    if (quantity > amountPossibleToTake) // Wenn wir MEHR Zucker haben, als noch Platz ist ...
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.MaxStackSize);  // ... dann fülle den Stapel komplett auf (bis MaxStackSize). Überschüssige Items werden später behandelt.
                    }
                    else
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + quantity);
                        InformAboutChange();
                        return 0;
                    }
                }
            }

            while (quantity > 0 && IsInventoryFull() == false)
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFirstFreeSlot(item, newQuantity);
            }
            return quantity;
        }

        public void AddItem(InventoryItem item)
        {
            AddItem(item.item, item.quantity);
        }

        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    continue;
                }
                returnValue[i] =  inventoryItems[i];
            }
            return returnValue;
        }

        public InventoryItem GetItemAt(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }

        public void SwapItems(int itemIndex1, int itemIndex2)
        {
            (inventoryItems[itemIndex1], inventoryItems[itemIndex2]) = (inventoryItems[itemIndex2], inventoryItems[itemIndex1]);
            InformAboutChange();
        }

        private void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }
    }

    [Serializable]
    public struct InventoryItem
    {
        public int quantity;
        public ItemSO item;
        public bool IsEmpty => item == null;

        public InventoryItem ChangeQuantity(int newQuantity)
        {
            return new InventoryItem
            {
                item = this.item,
                quantity = newQuantity,
            };
        }

        public static InventoryItem GetEmptyItem() => new InventoryItem
        {
            item = null,
            quantity = 0,
        };
    } 

}
