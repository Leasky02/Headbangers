using UnityEngine;

public class Player : MonoBehaviour
{
    private int _playerIndex = -1;

    private bool _isCameraTarget = true;

    private PlayerState _playerState = new PlayerState();

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
        gameObject.transform.GetChild(0).gameObject.GetComponent<PlayerColor>().Init();
    }

    private bool IsInitialized()
    {
        return _playerIndex > -1;
    }

    public void Update()
    {
        if (!IsInitialized())
            return;

        // Rename player at runtime appropriately
        gameObject.name = "Player " + (GetPlayerConfiguration().GetUserIndex() + 1);
    }

    private PlayerConfiguration GetPlayerConfiguration()
    {
        if (!IsInitialized())
            return null;

        return PlayerConfigurationManager.Instance.GetPlayerConfiguration(_playerIndex);
    }

    public int GetPlayerIndex()
    {
        return GetPlayerConfiguration().PlayerIndex;
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
        return _playerState.IsKnockedOut;
    }

    public void SetKnockedOut(bool knockedOut)
    {
        _playerState.IsKnockedOut = knockedOut;
    }

    public bool IsDead()
    {
        return _playerState.IsDead;
    }

    public void SetDead(bool dead)
    {
        _playerState.IsDead = dead;
    }
}

public class PlayerState
{
    public bool IsKnockedOut { get; set; }
    public bool IsDead { get; set; }
}
