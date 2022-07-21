using UnityEngine;

namespace LS
{

    public abstract class ISpawnPlayer<TPlayerConfiguration> : MonoBehaviour where TPlayerConfiguration : IPlayerConfiguration
    {
        public abstract void SpawnPlayer(TPlayerConfiguration playerConfig);
    }

}
