using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private UIInventoryPage inventoryUI;

    public int inventorySize = 10;
    private bool _isInventoryOpen = false;
    

    private void Start()
    {
        GameInput.Instance.OnInventoryToggle += GameInput_OnInventoryToggle;
        inventoryUI.Hide();
        
        inventoryUI.InitializeInventoryUI(inventorySize);
    }

    private void GameInput_OnInventoryToggle(object sender, System.EventArgs e)
    {
        if (_isInventoryOpen)
        {
            inventoryUI.Hide();
        }
        else
        {
            inventoryUI.Show();
        }
        _isInventoryOpen = !_isInventoryOpen;
    }
}