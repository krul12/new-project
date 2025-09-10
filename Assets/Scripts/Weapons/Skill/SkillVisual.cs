using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SkillVisual : MonoBehaviour
{
    [SerializeField] private Skill skill;
    
    private const string Cast = "Cast";
    private static readonly int Cast1 = Animator.StringToHash(Cast);
    
    private Sprite _defaultSprite;
    private Animator _animator;
    
    private SpriteRenderer _spriteRenderer;
    
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultSprite = _spriteRenderer.sprite; 
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _spriteRenderer.enabled = true;
        
        if (skill != null)
        {
            skill.OnSkillCast += Skill_OnSkillCast;
        }
    }
    
    public void TriggerEndAttackAnimation()
    {
        skill.AttackColliderTurnOff();
    }
    
    private void Skill_OnSkillCast(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(Cast1);
    }
    
    private void ResetSprite()
    {
        _spriteRenderer.sprite = _defaultSprite;
    }
    
    private void OnDisable()
    {
        ResetSprite();
    }
    
    
}
