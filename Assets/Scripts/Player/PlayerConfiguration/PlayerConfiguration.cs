using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfiguration : LS.IPlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi) : base(pi)
    {

    }

    public int PlayerColorID { get; set; }

    public string DisplayName { get; set; }
}
