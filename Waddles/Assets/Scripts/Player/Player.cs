using UnityEngine;

public class Player : MonoBehaviour
{
    private int _playerIndex = -1;

    private bool _isCameraTarget = true;

    private const string Tag = "Player";

    public static Player GetPlayerComponent(GameObject gameObject)
    {
        return LS.Helpers.GetComponentInParentWithTag<Player>(gameObject, Tag);
    }

    public void Init(int playerIndex)
    {
        if (_playerIndex > -1)
        {
            Debug.LogError("Player already initialized");
            return;
        }

        gameObject.tag = Tag;
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

    public bool IsCameraTarget()
    {
        return _isCameraTarget;
    }

    public void SetIsCameraTarget(bool isCameraTarget)
    {
        _isCameraTarget = isCameraTarget;
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
