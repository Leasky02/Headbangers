using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnPlayer_Lobby : LS.ISpawnPlayer<PlayerConfiguration>
{
    public override void SpawnPlayer(PlayerConfiguration playerConfig)
    {
        int playerIndex = playerConfig.PlayerIndex;
        PlayerInput playerInput = playerConfig.Input;

        playerInput.transform.GetComponent<Player>().Init(playerIndex);

        SetPosition(playerConfig);
        SetInputMap(playerConfig);
        FreezePosition(playerConfig);
    }

    public override void ShufflePlayers(List<PlayerConfiguration> playerConfigs)
    {
        LobbyManager.Find().UpdateAllSpawnPoint();
        playerConfigs.ForEach(playerConfig =>
        {
            SetPosition(playerConfig);
        });
    }

    public void SetPosition(PlayerConfiguration playerConfig)
    {
        Player.GetPlayerComponent(playerConfig.Input.gameObject).SetPosition(LobbyManager.Find().GetSpawnPointForUserIndex(playerConfig.GetUserIndex()).GetPosition());
    }

    public void FreezePosition(PlayerConfiguration playerConfig)
    {
        Player.GetPlayerComponent(playerConfig.Input.gameObject).GetRigidbodyHip().constraints = RigidbodyConstraints.FreezeAll;
    }

    private void SetInputMap(PlayerConfiguration playerConfig)
    {
        playerConfig.Input.SwitchCurrentActionMap("Lobby");
    }
}
