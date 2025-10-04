using UnityEngine;
using System;

public class DestructiblePlant : MonoBehaviour
{

    public event EventHandler OnDestructibleTakeDamage;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
