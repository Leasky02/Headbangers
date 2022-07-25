using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerData : MonoBehaviour
{
    private PlayerInput playerInput;
    
    //if player is a camera target
    private bool isCameraTarget = true;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        //rename player at runtime appropriately
        gameObject.name = "Player " + (playerInput.user.index + 1);
    }

    public bool GetIsCameraTarget()
    {
        return isCameraTarget;
    }
    
    public void SetIsCameraTarget(bool state)
    {
        isCameraTarget = state;
    }
}
