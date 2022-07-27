using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfiguration : LS.IPlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi) : base(pi)
    {

    }

    public Color PlayerColor { get; set; }
}
