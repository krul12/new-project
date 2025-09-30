using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    public abstract class ItemSO : ScriptableObject
    {
        public int ID => GetInstanceID();
    
        [field: SerializeField] public bool IsStackable {get; set;} 
        [field: SerializeField] public int MaxStackSize { get; set; } = 1;
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] [field: TextArea] public string Description { get; set; }
        [field: SerializeField] public Sprite ItemImage { get; set; }
        [field: SerializeField] public List<ItemParameter> DefaultParametersList {get; set;}
    
    
    }

    [Serializable]

    public struct ItemParameter : IEquatable<ItemParameter>
    {
        public float value;
        public ItemParameterSO itemParameter;

        public bool Equals(ItemParameter other)
        {
            return other.itemParameter == itemParameter;
        }
    }
}
