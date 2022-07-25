using UnityEngine;

public class Player : MonoBehaviour
{
    private int _playerIndex = -1;

    public static Player GetPlayerComponent(GameObject gameObject)
    {
        return LS.Helpers.GetComponentInParentWithTag<Player>(gameObject, "Player");
    }

    public void Init(int playerIndex)
    {
        if (_playerIndex > -1)
        {
            Debug.LogError("Player already initialized");
            return;
        }

        _playerIndex = playerIndex;
        SetColor();
    }

    private PlayerState GetPlayerState()
    {
        if (_playerIndex == -1)
            return null;

        return PlayerConfigurationManager.Instance.GetPlayerState(_playerIndex);
    }

    private void SetColor()
    {
        gameObject.transform.GetChild(0).gameObject.GetComponent<PlayerColor>().DefaultColor(_playerIndex);
    }

    public bool IsKnockedOut()
    {
        return GetPlayerState().IsKnockedOut();
    }

    public void SetKnockedOut(bool knockedOut)
    {
        GetPlayerState().SetKnockedOut(knockedOut);
    }

    public bool IsDead()
    {
        return GetPlayerState().IsDead();
    }

    public void SetDead(bool dead)
    {
        GetPlayerState().SetDead(dead);
    }
}
