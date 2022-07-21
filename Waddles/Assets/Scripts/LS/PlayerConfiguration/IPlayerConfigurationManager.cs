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
            DisableJoining();
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

        void OnPlayerJoined(PlayerInput pi)
        {
            Debug.Log("Player Joined " + pi.playerIndex);
            if (!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
            {
                // Set as child of this
                pi.transform.SetParent(transform);
                TPlayerConfiguration playerConfig = ConstructPlayerConfig(pi);
                playerConfigs.Add(playerConfig);
                SpawnPlayer(playerConfig);
            }
        }

        void OnPlayerLeft(PlayerInput pi)
        {
            Debug.Log("Player Left " + pi.playerIndex);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            playerConfigs.ForEach(playerConfig => SpawnPlayer(playerConfig));
        }

        protected virtual TPlayerConfiguration ConstructPlayerConfig(PlayerInput pi)
        {
            return new IPlayerConfiguration(pi) as TPlayerConfiguration;
        }

        protected TPlayerConfiguration GetPlayerConfiguration(int index)
        {
            return playerConfigs.Find(p => p.PlayerIndex == index);
        }

        private void SpawnPlayer(TPlayerConfiguration playerConfig)
        {
            GameObject spawnPlayerObj = GameObject.FindGameObjectWithTag("SpawnPlayer");
            if (spawnPlayerObj != null)
            {
                spawnPlayerObj.GetComponent<ISpawnPlayer<TPlayerConfiguration>>().SpawnPlayer(playerConfig);
            }
        }

        public void ReadyPlayer(int index)
        {
            TPlayerConfiguration playerConfig = GetPlayerConfiguration(index);
            if (playerConfig != null)
            {
                playerConfig.IsReady = true;
            }
        }

        public bool AllPlayersReady()
        {
            return playerConfigs.All(p => p.IsReady == true);
        }

        public bool CanAddPlayers()
        {
            return playerConfigs.Count < MaxPlayerCount();
        }
    }

}