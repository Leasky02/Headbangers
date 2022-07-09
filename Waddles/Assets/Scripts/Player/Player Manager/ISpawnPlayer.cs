using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ISpawnPlayer : MonoBehaviour
{
    public virtual void SpawnPlayer(PlayerConfiguration playerConfig)
    {
        Debug.Log("base spawn");
    }

    public virtual void SetupPlayer(PlayerConfiguration playerConfig)
    {
        Debug.Log("base setup");
    }
    public virtual void ShufflePlayer(PlayerConfiguration playerConfig, int oldIndex)
    {
        Debug.Log("base setup");
    }
}
