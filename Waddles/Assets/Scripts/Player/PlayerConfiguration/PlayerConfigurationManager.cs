using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigurationManager : LS.IPlayerConfigurationManager<PlayerConfigurationManager, PlayerConfiguration>
{
    [SerializeField] private RoundManager roundManager;
    [SerializeField] private int requiredPlayers = 1;

    // TODO: can this be removed?
    protected override PlayerConfiguration ConstructPlayerConfig(PlayerInput pi)
    {
        return new PlayerConfiguration(pi);
    }

    private void Update()
    {
        if(roundManager.GetInLobby())
        {
            if (playerConfigs.Count < requiredPlayers)
            {
                roundManager.StopCountdown();
                return;
            }

            foreach (PlayerConfiguration config in playerConfigs)
            {
                if (!config.IsReady)
                {
                    if (roundManager != null)
                    {
                        roundManager.StopCountdown();
                        return;
                    }
                }
            }

            if (!roundManager.GetCountdownInProgress())
                roundManager.PlayersReady();
        }
    }

    public void PlayerLeave(PlayerConfiguration playerConfiguration)
    {
        playerConfigs.Remove(playerConfiguration);

        // TODO: should we remove the player from the PlayerInputManager? and make a method on the base class to handle this.

        Invoke("ShufflePlayers", 0.05f);
    }

    private void ShufflePlayers()
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
