using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGameplayInputHandler : MonoBehaviour
{
    private Player player;

    public void Start()
    {
        player = Player.GetPlayerComponent(gameObject);
    }

    public void HandleAction_Gameplay_ShowPlayerNames(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DisplayNameRenderingManager.Instance.AddPlayerIndex(player.GetPlayerIndex());
        }
        else if (context.canceled)
        {
            DisplayNameRenderingManager.Instance.RemovePlayerIndex(player.GetPlayerIndex());
        }
    }

    public void HandleAction_Gameplay_Move(InputAction.CallbackContext context)
    {
        Vector2 normalizedDirection = context.ReadValue<Vector2>();
        if (normalizedDirection.sqrMagnitude > 0.16f)
        {
            player.GetComponent<PlayerWalk>().SetMoveDirection(normalizedDirection);
        }
        else
        {
            player.GetComponent<PlayerWalk>().SetMoveDirection(new Vector2(0, 0));
        }
    }

    public void HandleAction_Gameplay_Sit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player.GetComponent<PlayerSit>().AttemptSit();
        }
        else if (context.canceled)
        {
            player.GetComponent<PlayerSit>().StopSitting();
        }
    }

    public void HandleAction_Gameplay_Headbutt(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        player.GetComponent<PlayerHeadbutt>().AttemptHeadbutt();
    }

    public void HandleAction_Gameplay_Kick(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        player.GetComponent<PlayerKick>().AttemptKick();
    }

    public void HandleAction_Gameplay_Jump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        player.GetComponent<PlayerJump>().AttemptJump();
    }
}
