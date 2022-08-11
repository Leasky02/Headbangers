using System.Collections.Generic;

public class SpawnPlayer_Lobby : LS.ISpawnPlayer<PlayerConfiguration>
{
    public override void SpawnPlayer(PlayerConfiguration playerConfig)
    {
        playerConfig.GetPlayer().Init(playerConfig.PlayerIndex);

        SetPosition(playerConfig);

        Player.GetPlayerComponent(playerConfig.Input.gameObject).GetComponent<PlayerBody>().FreezePosition();

        playerConfig.Input.SwitchCurrentActionMap("Lobby");
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
}
