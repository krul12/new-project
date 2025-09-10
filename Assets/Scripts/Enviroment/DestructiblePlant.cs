using UnityEngine;
using System;

public class DestructiblePlant : MonoBehaviour
{

    public event EventHandler OnDestructibleTakeDamage;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Weapon>())
        {
            OnDestructibleTakeDamage?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
            
            NavMeshSurfaceManagement.Instance.RebakeNavmeshSurface();
        }
    }
}
