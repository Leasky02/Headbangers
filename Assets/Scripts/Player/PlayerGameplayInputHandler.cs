using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGameplayInputHandler : MonoBehaviour
{
    public void HandleAction_Gameplay_ShowPlayerNames(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DisplayNameRenderingManager.Instance.AddPlayerIndex(Player.GetPlayerComponent(gameObject).GetPlayerIndex());
        }
        else if (context.canceled)
        {
            DisplayNameRenderingManager.Instance.RemovePlayerIndex(Player.GetPlayerComponent(gameObject).GetPlayerIndex());
        }
    }
}
