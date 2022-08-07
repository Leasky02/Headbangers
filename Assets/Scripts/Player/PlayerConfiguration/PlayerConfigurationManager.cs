using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfigurationManager : LS.IPlayerConfigurationManager<PlayerConfigurationManager, PlayerConfiguration>
{
    protected override PlayerConfiguration ConstructPlayerConfig(PlayerInput pi)
    {
        return new PlayerConfiguration(pi);
    }

    protected override void AfterPlayerRemoved()
    {
        UpdatePlayerOrder();
    }

    private void UpdatePlayerOrder()
    {
        // TODO: only do this when in the lobby
        GameObject spawnPlayerObject = GameObject.FindGameObjectWithTag("SpawnPlayer");
        spawnPlayerObject.GetComponent<LS.ISpawnPlayer<PlayerConfiguration>>().ShufflePlayers(playerConfigs);
    }

    public int GetPlayerColorID(int playerIndex)
    {
        return GetPlayerConfiguration(playerIndex).PlayerColorID;
    }

    public void SetPlayerColorID(int playerIndex, int colorID)
    {
        PlayerConfiguration playerConfig = GetPlayerConfiguration(playerIndex);
        if (playerConfig != null)
        {
            playerConfig.PlayerColorID = colorID;
        }
    }

    public string GetPlayerDisplayName(int playerIndex)
    {
        return GetPlayerConfiguration(playerIndex).DisplayName;
    }

    public void SetPlayerDisplayName(int playerIndex, string name)
    {
        PlayerConfiguration playerConfig = GetPlayerConfiguration(playerIndex);
        if (playerConfig != null)
        {
            playerConfig.DisplayName = name;
        }
    }
}
