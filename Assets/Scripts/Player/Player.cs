using UnityEngine;

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
        AssignColor(nextColorID);
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

    public int GetUserIndex()
    {
        return GetPlayerConfiguration().GetUserIndex();
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

    public void AssignColor(int colorID)
    {
        PlayerConfigurationManager.Instance.SetPlayerColorID(m_playerIndex, colorID);
        Color playerColor = PlayerColorManager.Instance.GetColor(colorID);

        transform.GetChild(0).GetComponent<PlayerColor>().ApplyColor(playerColor);
    }
}

public class PlayerState
{
    public bool IsKnockedOut { get; set; }
    public bool IsDead { get; set; }
}
