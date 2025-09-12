using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(PolygonCollider2D))]
public class Skill : Weapon
{
    [SerializeField] private float castColliderTime = 0.2f;
    private const float CastVisualTime = 0.35f;
    

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
        
        if (!Player.Instance.UseMana(manaCost))
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
        PlayerVisual.Instance.spriteRendererPlayerVisual.enabled = false;
        InvokeAttackEvent();  
        OnSkillCast?.Invoke(this, EventArgs.Empty);
        
        yield return new WaitForSeconds(castColliderTime);
        AttackColliderTurnOffOn();
        
        yield return new WaitForSeconds(CastVisualTime);
        PlayerVisual.Instance.spriteRendererPlayerVisual.enabled = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out EnemyEntity enemyEntity))
        {
            enemyEntity.TakeDamage(damageAmount);
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

    private void OnDisable()
    {
        PlayerVisual.Instance.spriteRendererPlayerVisual.enabled = true;
    }
    
}
