using UnityEngine;

public class IPlayerHeadbuttedHandler
{

    public void InitOnSceneLoad()
    {
    }

    public virtual void HandleHeadbutted(Player headbuttedPlayer, Player headbuttingPlayer)
    {
        if (!headbuttedPlayer.CanBeHeadbutted())
            return;

        Debug.Log("Player headbutted" + headbuttedPlayer.GetUserIndex());

        // Change headbutting players face
        headbuttingPlayer.GetComponent<PlayerFace>().AttackingFace();

        // Calculate damage
        bool fightingBack = headbuttingPlayer.GetComponentInChildren<PlayerActions>().IsHeadbutting(); // TODO: improve
        float damage = CalculateDamage(headbuttingPlayer) * (fightingBack ? 0.5f : 1); // TODO: improve

        // Reduce headbutted players health
        PlayerHealth playerHealth = headbuttedPlayer.GetComponent<PlayerHealth>();
        playerHealth.ReduceHealth(damage);

        // Stop headbutted player from being headbutted again too soon
        headbuttedPlayer.PreventFromBeingHeadbutted();

        if (playerHealth.Health == 0)
        {
            // Knock the headbutted player out
            Gameplay.Instance.PlayerKnockedOutHandler.HandleKnockedOut(headbuttedPlayer, headbuttedPlayer.gameObject);
        }
        else
        {
            // Knock the headbutted player back
            headbuttedPlayer.GetComponentInChildren<PlayerKO>().KnockBack(headbuttingPlayer.GetPosition());
            headbuttedPlayer.GetComponent<PlayerFace>().UpdateFaceBasedOnHealth();
            float graceTime = headbuttedPlayer.GetComponentInChildren<PlayerKO>().GetGraceTime(); // TODO: improve
            headbuttedPlayer.Invoke("EnableBeingHeadbutted", graceTime * 0.5f); // TODO: improve
        }
    }

    private float CalculateDamage(Player headbuttingPlayer)
    {
        float angle = headbuttingPlayer.GetComponentInChildren<PlayerActions>().GetBodyAngle(); // TODO: improve
        angle = Mathf.Clamp(angle, 0f, 75f);

        PlayerKO playerKO = headbuttingPlayer.GetComponentInChildren<PlayerKO>(); // TODO: improve
        Vector2 pointA = new Vector2(playerKO.GetMinimumAngle(), playerKO.GetMinimumDamage());
        Vector2 pointB = new Vector2(playerKO.GetMaximumAngle(), playerKO.GetMaximumDamage());
        float m = ((pointA.y - pointB.y) / (pointA.x - pointB.x));
        float c = pointA.y - m * pointA.x;
        float damage = m * angle + c;
        return damage;
    }
}
