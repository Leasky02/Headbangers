using System.Collections.Generic;

public class DisplayNameRenderingManager : Singleton<DisplayNameRenderingManager>
{
    private List<int> m_playerShowingDisplayNames;

    public void Start()
    {
        m_playerShowingDisplayNames = new List<int>();
    }

    public void AddPlayerIndex(int playerIndex)
    {
        if (!m_playerShowingDisplayNames.Contains(playerIndex))
        {
            m_playerShowingDisplayNames.Add(playerIndex);
        }
    }

    public void RemovePlayerIndex(int playerIndex)
    {
        if (m_playerShowingDisplayNames.Contains(playerIndex))
        {
            m_playerShowingDisplayNames.Remove(playerIndex);
        }
    }

    public bool ShouldRenderDisplayNames()
    {
        return m_playerShowingDisplayNames.Count > 0;
        // Below is an alternative approach
        // bool allPlayersNotPerforming_ShowPlayerNames_Action = PlayerConfigurationManager.Instance.GetPlayerConfigurations().All(playerConfig =>
        // {
        //     return !playerConfig.Input.actions["ShowPlayerNames"].inProgress;
        // });
        // return !allPlayersNotPerforming_ShowPlayerNames_Action;
    }
}
