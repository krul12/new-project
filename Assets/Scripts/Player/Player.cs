using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    [Header("Health")] [SerializeField] private float maxHealth;
    [Header("DamageCD")] [SerializeField] private float damageRecoveryTime = 0.5f;
    [Header("Magic")] [SerializeField] private float maxMana;

    public Image fullHpBar;
    public Image fullManaBar;
    public static Player Instance { get; private set; }
    private PlayerMovement _pMovement;
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnFlashBlink;

    private float _currentPlayerHealth;
    private float _currentPlayerMana;
    private bool _canTakeDamage;
    private bool _isAlive = true;

    private Rigidbody2D _rb;
    private Camera _mainCamera;

    private void Awake()
    {
        _currentPlayerHealth = maxHealth;
        _currentPlayerMana = maxMana;
        UpdateHpBar();
        UpdateManaBar();
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
        
    }

    private void Start()
    {
        _canTakeDamage = true;
        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        _pMovement = GetComponent<PlayerMovement>();
    }
    
    public void AddHealth(float healthBoost)
    {
        _currentPlayerHealth += healthBoost;
        _currentPlayerHealth = Mathf.Clamp(_currentPlayerHealth, 0, maxHealth);
        UpdateHpBar();
    }
    
    public void AddMana(float manaBoost)
    {
        _currentPlayerMana += manaBoost;
        _currentPlayerMana = Mathf.Clamp(_currentPlayerMana, 0, maxMana);
        UpdateManaBar();
    }

    public bool IsAlive()
    {
        return _isAlive;
    }

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
            _currentPlayerHealth = Mathf.Max(0, _currentPlayerHealth -= damage);
            UpdateHpBar();
            _pMovement.ApplyKnockBack(damageSource);

            OnFlashBlink?.Invoke(this, EventArgs.Empty);

            StartCoroutine(DamageRecoveryRoutine());
            
        }

        DetectDeath();
    }
    
    public bool UseMana(float amount)
    {
        if (_currentPlayerMana >= amount)
        {
            _currentPlayerMana -= amount;
            UpdateManaBar(); 
            return true;
        }
        else
        {
            return false; 
        }
    }

    
    private bool CanTakeHit()
    {
        return _canTakeDamage && _isAlive && !_pMovement.IsDashing; //dash
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        _canTakeDamage = true;
    }

    private void DetectDeath()
    {
        if (_currentPlayerHealth <= 0 && _isAlive)
        {
            GameInput.Instance.DisableInput();
            _pMovement.DisableMovement();
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
            _isAlive = false;
        }
    }

    private void GameInput_OnPlayerAttack(object sender, EventArgs e)
    {
        ActiveWeapon.Instance.CurrentWeapon.Attack();
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnPlayerAttack -= GameInput_OnPlayerAttack;
    }

    private void UpdateHpBar()
    {
        fullHpBar.fillAmount = _currentPlayerHealth / maxHealth;
    }

    private void UpdateManaBar()
    {
        fullManaBar.fillAmount = _currentPlayerMana / maxMana;
    }
}