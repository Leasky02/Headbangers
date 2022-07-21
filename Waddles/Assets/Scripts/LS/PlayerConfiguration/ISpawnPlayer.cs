using UnityEngine;

namespace LS
{

    public abstract class ISpawnPlayer<TPlayerConfiguration> : MonoBehaviour where TPlayerConfiguration : IPlayerConfiguration
    {
        public abstract void SpawnPlayer(TPlayerConfiguration playerConfig);

        // TODO: remove form this class, ISpawnPlayer should only be responsible for spawning the player / setting it up in each scene that loads or when the player joins in that scene.
        public virtual void SetupPlayer(TPlayerConfiguration playerConfig)
        {
            Debug.Log("base setup");
        }
        
        // TODO: remove form this class, ISpawnPlayer should only be responsible for spawning the player / setting it up in each scene that loads or when the player joins in that scene.
        public virtual void ShufflePlayer(TPlayerConfiguration playerConfig, int oldIndex)
        {
            Debug.Log("base setup");
        }
    }

}
