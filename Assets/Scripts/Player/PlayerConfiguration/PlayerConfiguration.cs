using UnityEngine.InputSystem;

public class PlayerConfiguration : LS.IPlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi) : base(pi)
    {

    }

    public int PlayerColorID { get; set; }

    public string DisplayName { get; set; }

    public Player GetPlayer()
    {
        return Input.GetComponent<Player>();
    }
}
