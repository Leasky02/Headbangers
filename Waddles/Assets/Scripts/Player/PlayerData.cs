using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    //player ID
    [SerializeField] private int playerID;
    //color of player body
    [SerializeField] private Color playerColor;
    //if player is knocked out
    private bool knockedOut = false;
    //if player is dead
    private bool dead = false;

    private void Start()
    {
        //rename player at runtime appropriately
        gameObject.name = "Player " + playerID;
    }

    //return player ID
    public int GetPlayerID()
    {
        return playerID;
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
    //return player color
    public Color GetPlayerColor()
    {
        return playerColor;
    }

    public void SetKnockedOut(bool state)
    {
        knockedOut = state;
    }
    public void SetDead(bool state)
    {
        dead = state;
    }
    public void SetPlayerColor(Color newColor)
    {
        playerColor = newColor;
    }
}
