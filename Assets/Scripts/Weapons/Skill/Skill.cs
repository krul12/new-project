using UnityEngine;
using System;
using System.Collections;
using Misc;

[RequireComponent(typeof(PolygonCollider2D))]
public class Skill : Weapon
{
    [SerializeField] private float castColliderTime = 0.2f;
    
    public event EventHandler OnSkillCast;
    
    private PolygonCollider2D _polygonCollider2D;
    
    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
    }
    
    private void Start()
    {
        AttackColliderTurnOff();
    }
    
    public override void Attack()
    {
        if (!CanAttack())
        {
            return;
        }

        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(CastRoutine());
        }
    }

    private IEnumerator CastRoutine()
    {
        InvokeAttackEvent();  
        OnSkillCast?.Invoke(this, EventArgs.Empty);
        
        yield return new WaitForSeconds(castColliderTime);
        AttackColliderTurnOffOn();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Health enemyEntity))
        {
            enemyEntity.RemoveHealth(damageAmount);
        }
    }
    
    public void AttackColliderTurnOff()
    {
        _polygonCollider2D.enabled = false;
    }

    private void AttackColliderTurnOn()
    {
        _polygonCollider2D.enabled = true;
    }

    private void AttackColliderTurnOffOn()
    {
        AttackColliderTurnOff();
        AttackColliderTurnOn();
    }
}
