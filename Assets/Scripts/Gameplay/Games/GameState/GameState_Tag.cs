using UnityEngine;

public class GameState_Tag : IGameState
{
    private Player m_taggedPlayer;

    public override void OnGameStart()
    {
        // Tag a random player
        int playerCount = PlayerConfigurationManager.Instance.GetPlayerCount();
        int randomUserIndex = Random.Range(0, playerCount);
        Player randomPlayer = PlayerConfigurationManager.Instance.GetPlayerConfigurationByUserIndex(randomUserIndex).GetPlayer();
        TagPlayer(randomPlayer);
    }

    public void TagPlayer(Player player)
    {
        Debug.Log("Tagged Player:" + player.GetUserIndex());
        if (m_taggedPlayer)
        {
            m_taggedPlayer.GetComponent<PlayerColor>().ApplyColor(PlayerColorManager.Instance.GetColor(PlayerConfigurationManager.Instance.GetPlayerColorID(m_taggedPlayer.GetPlayerIndex())));
        }
        m_taggedPlayer = player;
        m_taggedPlayer.GetComponent<PlayerColor>().ApplyColor(new Color(1, 1, 1));
    }

    public Player GetTaggedPlayer()
    {
        return m_taggedPlayer;
    }
}