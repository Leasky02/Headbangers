using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfiguration : LS.IPlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi) : base(pi)
    {

    }

    public Color PlayerColor { get; set; }

    public PlayerState PlayerState { get; }
}

public class PlayerState
{    
    private bool _knockedOut = false; 

    private bool _dead = false;

    public bool IsKnockedOut()
    {
        return _knockedOut;
    }

    public void SetKnockedOut(bool knockedOut)
    {
        _knockedOut = knockedOut;
    }

    public bool IsDead()
    {
        return _dead;
    }

    public void SetDead(bool dead)
    {
        _dead = dead;
    }
}
