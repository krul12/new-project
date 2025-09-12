using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected int damageAmount;
    [SerializeField] protected float attackCooldown = 0.5f;
    [SerializeField] protected float manaCost;

    protected float LastAttackTime;
    
    public abstract void Attack();
    
    public event EventHandler OnWeaponAttack;

    protected bool CanAttack()
    {
        return Time.time >= LastAttackTime + attackCooldown;
    }

    protected void InvokeAttackEvent()
    {
        LastAttackTime = Time.time;
        OnWeaponAttack?.Invoke(this, EventArgs.Empty);
    }
}
