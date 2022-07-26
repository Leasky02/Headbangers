using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerReadyUp : MonoBehaviour
{
    [SerializeField] private PlayerConfiguration myPlayerConfig;

    public void SetupReadyUp(PlayerConfiguration playerconfig)
    {
        myPlayerConfig = playerconfig;
    }

    public void ReadyUp(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            myPlayerConfig.IsReady = !myPlayerConfig.IsReady;
        }
    }

    public void PlayerLeave(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            PlayerConfigurationManager.Instance.RemovePlayer(myPlayerConfig.PlayerIndex);
            PlayerColor.colorTaken[GetComponent<PlayerColor>().GetColourID()] = false;
            Destroy(transform.parent.gameObject);
        }
    }
}
