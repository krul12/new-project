using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerVisual : MonoBehaviour
{
    public static PlayerVisual Instance { get; private set; }
    
    private static readonly int Running = Animator.StringToHash(IsRunning);
    private static readonly int Death = Animator.StringToHash(IsDie);
    private static readonly int Dash = Animator.StringToHash(IsDashing);
    
    private const string IsRunning = "IsRunning";
    private const string IsDie = "IsDie";
    private const string IsDashing = "IsDashing";
    
    
    
    private Animator _animator;
    public SpriteRenderer spriteRendererPlayerVisual;
    private FlashBlink _flashBlink;
    
   

    private void Awake()
    {
        Instance = this;
        _animator = GetComponent<Animator>();
        spriteRendererPlayerVisual = GetComponent<SpriteRenderer>();
        _flashBlink = GetComponent<FlashBlink>();
        
    }

    private void Start()
    {
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
        
        
    }

    private void Update()
    {
        if (Player.Instance.IsAlive())
        {
            AdjustPlayerFacingDirection();
        }
        _animator.SetBool(Running, Player.Instance.IsRunning());
        _animator.SetBool(Dash, Player.Instance.IsDashing());
    }
    
    private void Player_OnPlayerDeath(object sender, System.EventArgs e)
    {
        _animator.SetBool(Death, true);
        _flashBlink.StopBlinking();
    }
    
    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerPosition = Player.Instance.GetPlayerScreenPosition();

        spriteRendererPlayerVisual.flipX = mousePos.x < playerPosition.x;
    }
    
    private void OnDestroy()
    {
        Player.Instance.OnPlayerDeath -= Player_OnPlayerDeath;
    }
    
}
