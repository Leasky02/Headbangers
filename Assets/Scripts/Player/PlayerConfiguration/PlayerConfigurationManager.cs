using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfigurationManager : LS.IPlayerConfigurationManager<PlayerConfigurationManager, PlayerConfiguration>
{
    // TODO: can this be removed?
    protected override PlayerConfiguration ConstructPlayerConfig(PlayerInput pi)
    {
        return new PlayerConfiguration(pi);
    }

    protected override void AfterPlayerRemoved()
    {
        Invoke("UpdatePlayerOrder", 0.05f);
    }

    private void UpdatePlayerOrder()
    {
        playerConfigs.ForEach(playerConfig =>
        {
            GameObject spawnPlayerObject = GameObject.FindGameObjectWithTag("SpawnPlayer");
            spawnPlayerObject.GetComponent<LS.ISpawnPlayer<PlayerConfiguration>>().ShufflePlayer(playerConfig);
        });
    }

    public Color GetPlayerColor(int playerIndex)
    {
        return GetPlayerConfiguration(playerIndex).PlayerColor;
    }

    public void SetPlayerColor(int playerIndex, Color color)
    {
        PlayerConfiguration playerConfig = GetPlayerConfiguration(playerIndex);
        if (playerConfig != null)
        {
            playerConfig.PlayerColor = color;
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
