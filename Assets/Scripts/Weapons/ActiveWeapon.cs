using Unity.VisualScripting;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] private Weapon[] weapons;
    private int _currentWeaponIndex; 

    public static ActiveWeapon Instance { get; private set; }
    public Weapon CurrentWeapon => weapons[_currentWeaponIndex];

    private void Awake()
    { 
        Instance = this;   
    }
    
    private void Start()
    {
        
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
        GameInput.Instance.OnWeaponSwitch += GameInput_OnWeaponSwitch;
        EquipWeapon(0);
    }
    
    private void Update()
    {
            FollowMousePosition();
        
    }
    
    private void GameInput_OnWeaponSwitch(object sender, int index)
    {
        EquipWeapon(index);
    }
    
    private void EquipWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length)
        {
            return;
        }

        foreach (Weapon weapon in weapons)
        {
            weapon.gameObject.SetActive(false);
        }
        
        
        
        _currentWeaponIndex = index;
        weapons[_currentWeaponIndex].gameObject.SetActive(true);
    }
    
    private void Player_OnPlayerDeath(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }

    private void FollowMousePosition()
    {
        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerPosition = Player.Instance.GetPlayerScreenPosition();

        transform.rotation = Quaternion.Euler(0, mousePos.x < playerPosition.x ? 180 : 0, 0);
    }
    
    private void OnDestroy()
    {
        Player.Instance.OnPlayerDeath -= Player_OnPlayerDeath;
        GameInput.Instance.OnWeaponSwitch -= GameInput_OnWeaponSwitch;
    }
   
}
