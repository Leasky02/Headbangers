using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGameplayInputHandler : MonoBehaviour
{
    [SerializeField] private PlayerActions actions;

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

    public void HandleAction_Gameplay_Sit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            actions.AttemptSit();
        }
        else if (context.canceled)
        {
            actions.StopSitting();
        }
    }

    public void HandleAction_Gameplay_Headbutt(InputAction.CallbackContext context)
    {
    }

    public void HandleAction_Gameplay_Kick(InputAction.CallbackContext context)
    {
    }

    public void HandleAction_Gameplay_Jump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        actions.AttemptJump();
    }
}
