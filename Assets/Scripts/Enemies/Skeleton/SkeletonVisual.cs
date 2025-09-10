using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class SkeletonVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private EnemyEntity enemyEntity;
    [SerializeField] private GameObject enemyShadow;
    
    private const string IsRunning = "IsRunning";
    private const string ChasingSpeedMultiplier = "ChasingSpeedMultiplier";
    private const string Attack = "Attack";
    private const string Takehit = "TakeHit";
    private const string Death = "IsDie";
    
    private static readonly int Running = Animator.StringToHash(IsRunning);
    private static readonly int SpeedMultiplier = Animator.StringToHash(ChasingSpeedMultiplier);
    private static readonly int Attack1 = Animator.StringToHash(Attack);
    private static readonly int TakeHit = Animator.StringToHash(Takehit);
    private static readonly int Death1 = Animator.StringToHash(Death);

    private SpriteRenderer _spriteRenderer;
    
    

    private Animator _animator;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        enemyAI.OnEnemyAttack += enemyAI_OnEnemyAttack;
        enemyEntity.OnTakeHit += enemyAI_OnTakeHit;
        enemyEntity.OnDeath += enemyEntity_OnDeath;
    }

    private void enemyAI_OnTakeHit(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(TakeHit);
    }

    private void enemyEntity_OnDeath(object sender, System.EventArgs e)
    {
        _animator.SetBool(Death1, true);
        _spriteRenderer.sortingOrder = -1;
        enemyShadow.SetActive(false);
    }
    
    private void Update()
    {
        _animator.SetBool(Running, enemyAI.IsRunning);
        _animator.SetFloat(SpeedMultiplier, enemyAI.GetRoamingAnimationSpeed());
    }

    public void TriggerAttackAnimationTurnOff()
    {
        enemyEntity.PolygonColliderTurnOff();
    }

    public void TriggerAttackAnimationTurnOn()
    {
        enemyEntity.PolygonColliderTurnOn();
    }

    private void enemyAI_OnEnemyAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(Attack1);
    }
    
    private void OnDestroy()
    {
        enemyAI.OnEnemyAttack -= enemyAI_OnEnemyAttack;
        enemyEntity.OnTakeHit -= enemyAI_OnTakeHit;
        enemyEntity.OnDeath -= enemyEntity_OnDeath;
    }
}
