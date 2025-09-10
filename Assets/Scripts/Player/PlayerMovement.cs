using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Speed")] [SerializeField] private float movingSpeed = 5f;
    private const float MinMovementSpeed = 0.1f;
    private bool _isRunning = false;
    public bool IsRunning => _isRunning;
    private float _initialMovingSpeed;

    [SerializeField] private TrailRenderer trailRenderer;

    [Header("Dash")] [SerializeField] private int dashSpeed = 4;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCoolDownTime = 0.3f;
    private bool _canDash = true;
    public bool IsDashing => _isDashing;
    private bool _isDashing;


    private Rigidbody2D _rb;
    private Vector2 _inputVector;
    private PlayerInputActions _playerIa;
    private KnockBack _knockBack;


    private void Awake()
    {
        _initialMovingSpeed = movingSpeed;
        _rb = GetComponent<Rigidbody2D>();
        _knockBack = GetComponent<KnockBack>();

        _playerIa = new PlayerInputActions();
        _playerIa.Enable();
        _playerIa.Player.Move.performed += ctx => _inputVector = ctx.ReadValue<Vector2>();
        _playerIa.Player.Move.canceled += _ => _inputVector = Vector2.zero;
        _playerIa.Player.Dash.performed += Dash;
    }

    private void FixedUpdate()
    {
        if (_knockBack.IsGettingKnockedBack)
        {
            return;
        }

        HandleMovement();
    }

    private void HandleMovement()
    {
        _rb.MovePosition(_rb.position + _inputVector * (movingSpeed * Time.fixedDeltaTime));

        if (Mathf.Abs(_inputVector.x) > MinMovementSpeed || Mathf.Abs(_inputVector.y) > MinMovementSpeed)
        {
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }
    }

    public void ApplyKnockBack(Transform source)
    {
        _knockBack.GetKnockBack(source);
    }

    public void DisableMovement()
    {
        _playerIa.Disable();
        _canDash = false;
        _isDashing = false;
        _isRunning = false;
        _rb.linearVelocity = Vector2.zero;
    }

    //TODO Refactor Dash ability, create an abstract Ability class, extend it with Dash
    private void Dash(InputAction.CallbackContext ctx)
    {
        if (_canDash)
        {
            StartCoroutine(DashRoutine());
        }
    }

    private IEnumerator DashRoutine()
    {
        _isDashing = true;
        _canDash = false;
        movingSpeed *= dashSpeed;
        trailRenderer.emitting = true;
        ActiveWeapon.Instance.gameObject.SetActive(false);


        yield return new WaitForSeconds(dashTime);
        _isDashing = false;
        trailRenderer.emitting = false;
        movingSpeed = _initialMovingSpeed;
        ActiveWeapon.Instance.gameObject.SetActive(true);


        yield return new WaitForSeconds(dashCoolDownTime);
        _canDash = true;
    }
}