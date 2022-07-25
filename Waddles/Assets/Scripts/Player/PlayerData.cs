using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerData : MonoBehaviour
{
    private PlayerInput playerInput;
    
    //if player is knocked out
    private bool knockedOut = false;
    //if player is dead
    private bool dead = false;
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

    //return knockedOut state
    public bool GetKnockedOut()
    {
        return knockedOut;
    }
    //return dead state
    public bool GetDead()
    {
        return dead;
    }
    public bool GetIsCameraTarget()
    {
        return isCameraTarget;
    }

    public void SetKnockedOut(bool state)
    {
        knockedOut = state;
    }
    public void SetDead(bool state)
    {
        dead = state;
    }
    
    public void SetIsCameraTarget(bool state)
    {
        isCameraTarget = state;
    }
}
