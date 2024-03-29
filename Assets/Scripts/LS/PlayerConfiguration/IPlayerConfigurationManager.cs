using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace LS
{


    // Component to sit next to PlayerInput.
    [RequireComponent(typeof(PlayerInputManager))]
    public abstract class IPlayerConfigurationManager<TPlayerConfigurationManager, TPlayerConfiguration> : Singleton<TPlayerConfigurationManager> where TPlayerConfigurationManager : IPlayerConfigurationManager<TPlayerConfigurationManager, TPlayerConfiguration> where TPlayerConfiguration : IPlayerConfiguration
    {
        protected List<TPlayerConfiguration> playerConfigs;

        public override void Awake()
        {
            base.Awake();

            playerConfigs = new List<TPlayerConfiguration>();
            EnableJoining();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void EnableJoining()
        {
            GetComponent<PlayerInputManager>().EnableJoining();
        }

        public void DisableJoining()
        {
            GetComponent<PlayerInputManager>().DisableJoining();
        }

        public int MaxPlayerCount()
        {
            return GetComponent<PlayerInputManager>().maxPlayerCount;
        }

        public void OnPlayerJoined(PlayerInput pi)
        {
            Debug.Log("Player Joined, playerIndex: " + pi.playerIndex + ", user.index: " + pi.user.index);
            if (!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
            {
                // Set as child of this
                pi.transform.SetParent(transform);
                TPlayerConfiguration playerConfig = ConstructPlayerConfig(pi);
                playerConfigs.Add(playerConfig);
                SpawnPlayer(playerConfig);
            }
        }

        public void OnPlayerLeft(PlayerInput pi)
        {
            Debug.Log("Player Left, playerIndex: " + pi.playerIndex + ", user.index: " + pi.user.index);
            // TODO: handle player connection lost
        }

        public void RemovePlayer(int playerIndex, GameObject gameObjectToDestroy = null)
        {
            if (gameObjectToDestroy != null)
            {
                Destroy(gameObjectToDestroy);
            }
            TPlayerConfiguration playerConfig = GetPlayerConfiguration(playerIndex);
            if (playerConfig != null)
            {
                playerConfigs.Remove(playerConfig);
                // TODO: also remove player from PlayerInputManager
                AfterPlayerRemoved();
            }
        }

        protected virtual void AfterPlayerRemoved()
        {

        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            playerConfigs.ForEach(playerConfig => SpawnPlayer(playerConfig));
        }

        protected virtual TPlayerConfiguration ConstructPlayerConfig(PlayerInput pi)
        {
            return new IPlayerConfiguration(pi) as TPlayerConfiguration;
        }

        public List<TPlayerConfiguration> GetPlayerConfigurations()
        {
            return playerConfigs;
        }

        public TPlayerConfiguration GetPlayerConfiguration(int playerIndex)
        {
            return playerConfigs.Find(p => p.PlayerIndex == playerIndex);
        }

        public TPlayerConfiguration GetPlayerConfigurationByUserIndex(int userIndex)
        {
            return playerConfigs.Find(p => p.GetUserIndex() == userIndex);
        }

        private void SpawnPlayer(TPlayerConfiguration playerConfig)
        {
            GameObject spawnPlayerObj = GameObject.FindGameObjectWithTag("SpawnPlayer");
            if (spawnPlayerObj != null)
            {
                spawnPlayerObj.GetComponent<ISpawnPlayer<TPlayerConfiguration>>().SpawnPlayer(playerConfig);
            }
        }

        public bool IsPlayerReady(int playerIndex)
        {
            return GetPlayerConfiguration(playerIndex).IsReady;
        }

        public void SetPlayerReady(int playerIndex, bool ready = true)
        {
            TPlayerConfiguration playerConfig = GetPlayerConfiguration(playerIndex);
            if (playerConfig != null)
            {
                playerConfig.IsReady = ready;
            }
        }

        public void TogglePlayerReady(int playerIndex)
        {
            TPlayerConfiguration playerConfig = GetPlayerConfiguration(playerIndex);
            if (playerConfig != null)
            {
                playerConfig.IsReady = !playerConfig.IsReady;
            }
        }

        public void SetAllPlayerReady(bool ready = true)
        {
            playerConfigs.ForEach(config => config.IsReady = ready);
        }

        public bool AllPlayersReady()
        {
            return playerConfigs.All(p => p.IsReady == true);
        }

        public bool CanAddPlayers()
        {
            return playerConfigs.Count < MaxPlayerCount();
        }

        public int GetPlayerCount()
        {
            return playerConfigs.Count;
        }

        public void SwitchCurrentActionMap(string mapNameOrId)
        {
            playerConfigs.ForEach(config => config.Input.SwitchCurrentActionMap(mapNameOrId));
        }

        public void SwitchCurrentActionMap(int playerIndex, string mapNameOrId)
        {
            TPlayerConfiguration playerConfig = GetPlayerConfiguration(playerIndex);
            if (playerConfig != null)
            {
                playerConfig.Input.SwitchCurrentActionMap(mapNameOrId);
            }
        }
    }

}