using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance {get; private set;}
    private PlayerInputActions _playerIa;

    public event EventHandler OnInventoryToggle;
    public event EventHandler OnPlayerAttack;
    public event EventHandler<int> OnWeaponSwitch;
    public event EventHandler OnPlayerDash;
    public event EventHandler<float> OnHotbarScroll;
    
    private void Awake()
    {
        Instance = this;
        _playerIa = new PlayerInputActions();
        _playerIa.Enable();
        _playerIa.Combat.Attack.started += PlayerAttack_started;
        _playerIa.Combat.SwitchWeapon1.performed += ctx => OnWeaponSwitch?.Invoke(this, 0);
        _playerIa.Combat.SwitchWeapon2.performed += ctx => OnWeaponSwitch?.Invoke(this, 1);
        _playerIa.Combat.SwitchWeapon3.performed += ctx => OnWeaponSwitch?.Invoke(this, 2);
        _playerIa.Player.Dash.performed += PlayerDash_perfomed;
        _playerIa.UI.InventoryUI.performed += InventoryToggle_perfomed;
        _playerIa.Player.HotbarScroll.performed += ctx =>
        {
            float scroll = ctx.ReadValue<float>();
            OnHotbarScroll?.Invoke(this, scroll);
        };
    }

    public void DisableInput()
    {
        _playerIa.Disable();
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = _playerIa.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        return mousePos;
    }
    
    private void PlayerAttack_started(InputAction.CallbackContext obj)
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerDash_perfomed(InputAction.CallbackContext obj)
    {
        OnPlayerDash?.Invoke(this, EventArgs.Empty);
    }

    private void InventoryToggle_perfomed(InputAction.CallbackContext obj)
    {
        OnInventoryToggle?.Invoke(this, EventArgs.Empty);
    }
    
}
