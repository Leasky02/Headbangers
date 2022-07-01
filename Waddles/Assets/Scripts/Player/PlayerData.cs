using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private int playerID;
    [SerializeField] private Color playerColor;

    private bool knockedOut = false;
    private bool dead = false;

    private void Start()
    {
        gameObject.name = "Player " + playerID;
    }

    public int GetPlayerID()
    {
        return playerID;
    }
    public bool GetKnockedOut()
    {
        return knockedOut;
    }
    public bool GetDead()
    {
        return dead;
    }
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
