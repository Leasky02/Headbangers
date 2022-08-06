using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbodyHip;

    private int m_playerIndex = -1;

    private bool m_isCameraTarget = true;

    private PlayerState m_playerState = new PlayerState();

    private const string Tag = "Player";

    public static Player GetPlayerComponent(GameObject gameObject)
    {
        if (gameObject.tag == Tag)
        {
            return gameObject.GetComponent<Player>();
        }
        return LS.Helpers.GetComponentInParentWithTag<Player>(gameObject, Tag);
    }

    public void Init(int playerIndex)
    {
        if (m_playerIndex > -1)
        {
            Debug.LogError("Player already initialized");
            return;
        }

        gameObject.tag = Tag;
        m_playerIndex = playerIndex;

        int nextColorID = PlayerColorManager.Instance.TakeNextAvailableColorID();
        UpdateColor(nextColorID);
    }

    private bool IsInitialized()
    {
        return m_playerIndex > -1;
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

        return PlayerConfigurationManager.Instance.GetPlayerConfiguration(m_playerIndex);
    }

    public int GetPlayerIndex()
    {
        return GetPlayerConfiguration().PlayerIndex;
    }

    public bool IsCameraTarget()
    {
        return m_isCameraTarget;
    }

    public void SetIsCameraTarget(bool isCameraTarget)
    {
        m_isCameraTarget = isCameraTarget;
    }

    public Rigidbody GetRigidbodyHip()
    {
        return rigidbodyHip;
    }

    public void SetPosition(Vector3 position)
    {
        rigidbodyHip.transform.position = position;
    }

    public bool IsKnockedOut()
    {
        return m_playerState.IsKnockedOut;
    }

    public void SetKnockedOut(bool knockedOut)
    {
        m_playerState.IsKnockedOut = knockedOut;
    }

    public bool IsDead()
    {
        return m_playerState.IsDead;
    }

    public void SetDead(bool dead)
    {
        m_playerState.IsDead = dead;
    }

    private void UpdateColor(int colorID)
    {
        PlayerConfigurationManager.Instance.SetPlayerColorID(m_playerIndex, colorID);
        Color playerColor = PlayerColorManager.Instance.GetColor(colorID);

        transform.GetChild(0).GetComponent<PlayerColor>().ApplyColor(playerColor);
    }

    // Input Action Handlers

    public void HandleAction_Lobby_ReadyUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PlayerConfigurationManager.Instance.TogglePlayerReady(m_playerIndex);
        }
    }

    public void HandleAction_Lobby_Leave(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PlayerColorManager.Instance.SetColorAvailable(PlayerConfigurationManager.Instance.GetPlayerColorID(m_playerIndex));
            PlayerConfigurationManager.Instance.RemovePlayer(m_playerIndex, gameObject);
        }
    }

    public void HandleAction_Lobby_ColorUp(InputAction.CallbackContext context)
    {
        if (PlayerConfigurationManager.Instance.IsPlayerReady(m_playerIndex))
            return;

        if (!context.performed)
            return;

        if (PlayerColorManager.Instance.HasColorsAvailable())
        {
            int newColorID = PlayerColorManager.Instance.TakeNextAvailableColorID(PlayerConfigurationManager.Instance.GetPlayerColorID(m_playerIndex));
            UpdateColor(newColorID);
        }
    }

    public void HandleAction_Lobby_ColorDown(InputAction.CallbackContext context)
    {
        if (PlayerConfigurationManager.Instance.IsPlayerReady(m_playerIndex))
            return;

        if (!context.performed)
            return;

        if (PlayerColorManager.Instance.HasColorsAvailable())
        {
            int newColorID = PlayerColorManager.Instance.TakePreviousAvailableColorID(PlayerConfigurationManager.Instance.GetPlayerColorID(m_playerIndex));
            UpdateColor(newColorID);
        }
    }
}

public class PlayerState
{
    public bool IsKnockedOut { get; set; }
    public bool IsDead { get; set; }
}
