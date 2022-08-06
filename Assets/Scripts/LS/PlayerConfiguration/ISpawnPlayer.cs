using System.Collections.Generic;
using UnityEngine;

namespace LS
{

    public abstract class ISpawnPlayer<TPlayerConfiguration> : MonoBehaviour where TPlayerConfiguration : IPlayerConfiguration
    {
        public abstract void SpawnPlayer(TPlayerConfiguration playerConfig);

        // TODO: remove from this class, ISpawnPlayer should only be responsible for spawning the player / setting it up in each scene that loads or when the player joins in that scene.
        public virtual void ShufflePlayers(List<TPlayerConfiguration> playerConfigs)
        {
            Debug.Log("base setup");
        }
    }

}
