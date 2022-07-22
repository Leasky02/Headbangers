using UnityEngine;
using UnityEngine.InputSystem;

namespace LS
{

    // Component to sit next to PlayerInput.
    [RequireComponent(typeof(PlayerInput))]
    public class IPlayerConfiguration
    {
        public IPlayerConfiguration(PlayerInput pi)
        {
            Input = pi;
            PlayerIndex = pi.playerIndex;
        }

        public PlayerInput Input { get; set; }
        public int PlayerIndex { get; set; }
        public bool IsReady { get; set; }
    }

}