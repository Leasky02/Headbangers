using UnityEngine;

public class RoundManager : Singleton<RoundManager>
{
    private int roundNumber = 1;

    public int GetRoundNumber()
    {
        return roundNumber;
    }

    public void NextRound()
    {
        roundNumber++;
    }
}