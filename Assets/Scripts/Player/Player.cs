using System;
using System.Collections;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerMovement))]

public class Player : MonoBehaviour
{
    // [Header("Speed")]
    // [SerializeField] private float movingSpeed = 5f;
    [Header("Health")]
    [SerializeField] private int maxHealth = 10;
    [Header("DamageCD")]
    [SerializeField] private float damageRecoveryTime = 0.5f;
    // [Header("Dash")]
    // [SerializeField] private int dashSpeed = 4;
    // [SerializeField] private float dashTime = 0.2f;
    // [SerializeField] private float dashCoolDownTime = 0.3f;
    // [SerializeField] private TrailRenderer trailRenderer;
    public static Player Instance { get; private set; }
    private PlayerMovement _pMovement;
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnFlashBlink;
    
    // private const float MinMovementSpeed = 0.1f;
    // private bool _isRunning = false;
    private int _currentHealth;
    private bool _canTakeDamage;
    private bool _isAlive = true;
    // private float _initialMovingSpeed;
    // private bool _canDash;
    // private bool _isDashing;
    
    // private Vector2 _inputVector;
    private Rigidbody2D _rb;
    private Camera _mainCamera;
    // private KnockBack _knockBack;
    
    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
        // _knockBack = GetComponent<KnockBack>();
        
        // _initialMovingSpeed = movingSpeed;
    }
    
    private void Start()
    {
        // _canDash = true;
        _canTakeDamage = true;
        _currentHealth = maxHealth;
        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        _pMovement = GetComponent<PlayerMovement>();
        // GameInput.Instance.OnPlayerDash += GameInput_OnPlayerDash;
    }

    // private void Update()
    // {
    //     _inputVector = GameInput.Instance.GetMovementVector();
    // }

    // private void FixedUpdate()
    // {
    //    if (_knockBack.IsGettingKnockedBack)
        // {
        //     return;
        // }
        
        // HandleMovement();
    // }

    public bool IsAlive()
    {
        return _isAlive;
    }
    
    // public bool IsRunning()
    // {
    //     return _isRunning;
    // }

    // public bool IsDashing()
    // {
    //     return _isDashing;
    // }

    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = _mainCamera.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

    public void TakeDamage(Transform damageSource, int damage)
    {
        if (CanTakeHit())
        {
            
            _canTakeDamage = false;
            _currentHealth = Mathf.Max(0, _currentHealth -= damage);
            _pMovement.ApplyKnockBack(damageSource);
            // _knockBack.GetKnockBack(damageSource);
            
            OnFlashBlink?.Invoke(this, EventArgs.Empty);
            
            StartCoroutine(DamageRecoveryRoutine());
        }
        
        DetectDeath();
    }

    private bool CanTakeHit()
    {
        return _canTakeDamage && _isAlive && !_pMovement.IsDashing; //dash
    }
    
    // private void GameInput_OnPlayerDash(object sender, System.EventArgs e)
    // {
    //     Dash();
    // }

    // private void Dash()
    // {
    //     if (_canDash)
    //     {
    //         StartCoroutine(DashRoutine());
    //     }
    // }
    //
    // private IEnumerator DashRoutine()
    // {
    //     _isDashing = true;
    //     _canDash = false;
    //     movingSpeed *= dashSpeed;
    //     trailRenderer.emitting = true;
    //     ActiveWeapon.Instance.gameObject.SetActive(false);
    //     
    //     
    //     yield return new WaitForSeconds(dashTime);
    //     _isDashing  = false;
    //     trailRenderer.emitting = false;
    //     movingSpeed = _initialMovingSpeed;
    //     ActiveWeapon.Instance.gameObject.SetActive(true);
    //     
    //     
    //     yield return new WaitForSeconds(dashCoolDownTime);
    //     _canDash = true;
    //     
    // }
    
    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        _canTakeDamage = true;
    }

    private void DetectDeath()
    {
        if (_currentHealth <= 0 && _isAlive)
        {
            GameInput.Instance.DisableInput();
            _pMovement.DisableMovement();
            // _knockBack.StopKnockBackMovement();
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
            _isAlive = false;
        }
    }

    private void GameInput_OnPlayerAttack(object sender, EventArgs e)
    {
        Debug.Log("Player Attack");
        ActiveWeapon.Instance.CurrentWeapon.Attack();
    }

    // private void HandleMovement()
    // {
    //     _rb.MovePosition(_rb.position + _inputVector * (movingSpeed * Time.fixedDeltaTime));
    //
    //     if (Mathf.Abs(_inputVector.x) > MinMovementSpeed || Mathf.Abs(_inputVector.y) > MinMovementSpeed)
    //     {
    //         _isRunning = true;
    //     }
    //     else
    //     {
    //         _isRunning = false;
    //     }
    // }
    
    
    private void OnDestroy()
    {
        GameInput.Instance.OnPlayerAttack -= GameInput_OnPlayerAttack;
    }
    
}