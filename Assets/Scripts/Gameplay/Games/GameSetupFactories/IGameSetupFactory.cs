using UnityEngine;

public class IGameSetupFactory
{
    public virtual IPlayerRespawn CreatePlayerRespawn()
    {
        return new IPlayerRespawn();
    }
}