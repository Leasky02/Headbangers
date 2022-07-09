using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerReadyUp : MonoBehaviour
{
    [SerializeField] private PlayerConfiguration myPlayerConfig;

    private TextMesh readyUpText;

    [SerializeField] private Color green;

    private bool ready;

    public void SetupReadyUp(TextMesh text, PlayerConfiguration playerconfig)
    {
        myPlayerConfig = playerconfig;
        readyUpText = text;
    }

    public void ReadyUp(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            myPlayerConfig.IsReady = !myPlayerConfig.IsReady;
        }

        if(myPlayerConfig.IsReady)
        {
            readyUpText.text = ("Ready");
            readyUpText.color = green;
        }
        else
        {
            readyUpText.text = ("");
        }
    }
    
    public void TransferReadyUp()
    {
        if (myPlayerConfig.IsReady)
        {
            readyUpText.text = ("Ready");
            readyUpText.color = green;
        }
        else
        {
            readyUpText.text = ("");
        }
    }

    public void PlayerLeave(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            PlayerManager.Instance.PlayerLeave(myPlayerConfig);
            readyUpText.text = ("");
            PlayerColor.colorTaken[GetComponent<PlayerColor>().GetColourID()] = false;
            Destroy(transform.parent.gameObject);
        }
    }
}