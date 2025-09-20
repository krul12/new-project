using UnityEngine;
using System;
using Misc;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Health))]
public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private EnemySO enemySo;

    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;
    
    // private int _currentHealth;
    private Health _health;
    
    private PolygonCollider2D _polygonCollider2D;
    private BoxCollider2D _boxCollider2D;
    private EnemyAI _enemyAI;

    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _enemyAI = GetComponent<EnemyAI>();
        _health = GetComponent<Health>();
        _health.MaxHealth = enemySo.enemyHealth;
        _health.OnDie += Die;
        _health.OnHealthChanged += HealthChanged;
    }
    private void Start()
    {
        // _currentHealth = enemySo.enemyHealth;
    }
    
    public void PolygonColliderTurnOff()
    {
        _polygonCollider2D.enabled = false;
    }

    public void PolygonColliderTurnOn()
    {
        _polygonCollider2D.enabled = true;
    }

    // public void TakeDamage(int damage)
    // {
    //     _currentHealth -= damage;
    //     OnTakeHit?.Invoke(this, EventArgs.Empty);
    //     DetectDeath();
    // }
    
    public void HealthChanged(int currentHealth, int maxHealth)
    {
        // _currentHealth -= damage;
        OnTakeHit?.Invoke(this, EventArgs.Empty);
        // DetectDeath();
    }

    

    // private void DetectDeath()
    // {
    //     if (_currentHealth <= 0)
    //     {
    //         _boxCollider2D.enabled = false;
    //         _polygonCollider2D.enabled = false;
    //         _enemyAI.SetDeathState();
    //         OnDeath?.Invoke(this, EventArgs.Empty);
    //     }
    // }

    private void Die()
    {
        _boxCollider2D.enabled = false;
        _polygonCollider2D.enabled = false;
        _enemyAI.SetDeathState();
        OnDeath?.Invoke(this, EventArgs.Empty); //TODO WHat is that

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Player player))
        {
            player.TakeDamage(transform, enemySo.enemyDamageAmount);
        }
    }
}
