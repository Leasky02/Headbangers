using UnityEngine;

public class IPlayerKickedHandler
{

    public void InitOnSceneLoad()
    {
    }

    public virtual void HandleKicked(Player kickedPlayer, Player kickingPlayer)
    {
        Debug.Log("Player kicked" + kickedPlayer.GetUserIndex());

        // Change kicking players face
        kickingPlayer.GetComponent<PlayerFace>().AttackingFace();

        // Calculate damage
        float kickingDamage = CalculateDamage(kickingPlayer);

        // Reduce kicked players health
        PlayerHealth playerHealth = kickedPlayer.GetComponent<PlayerHealth>();
        playerHealth.ReduceHealth(kickingDamage);

        if (playerHealth.Health == 0)
        {
            // Knock the kicked player out
            Gameplay.Instance.PlayerKnockedOutHandler.HandleKnockedOut(kickedPlayer, kickedPlayer.gameObject);
        }
        else
        {
            // Knock the headbutted player back
            kickedPlayer.GetComponent<PlayerKO>().KnockBack(kickingPlayer.GetPosition());
            kickedPlayer.GetComponent<PlayerFace>().UpdateFaceBasedOnHealth();
        }
    }

    private float CalculateDamage(Player kickingPlayer)
    {
        return kickingPlayer.GetComponentInChildren<PlayerKO>().GetKickingDamage(); // TODO: improve
    }
}
