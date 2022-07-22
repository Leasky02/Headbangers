using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfigurationManager : LS.IPlayerConfigurationManager<PlayerConfigurationManager, PlayerConfiguration>
{
    // TODO: can this be removed?
    protected override PlayerConfiguration ConstructPlayerConfig(PlayerInput pi)
    {
        return new PlayerConfiguration(pi);
    }

    public void PlayerLeave(PlayerConfiguration playerConfiguration)
    {
        playerConfigs.Remove(playerConfiguration);

        // TODO: should we remove the player from the PlayerInputManager? and make a method on the base class to handle this.

        Invoke("UpdatePlayerOrder", 0.05f);
    }

    private void UpdatePlayerOrder()
    {
        //resort players in order
        foreach (PlayerConfiguration config in playerConfigs)
        {
            int oldIndex = config.PlayerIndex;
            config.PlayerIndex = config.Input.user.index;

            //reshuffle position
            GameObject spawnPlayerObject = GameObject.FindGameObjectWithTag("SpawnPlayer");
            spawnPlayerObject.GetComponent<LS.ISpawnPlayer<PlayerConfiguration>>().ShufflePlayer(config, oldIndex);
        }
    }
}
