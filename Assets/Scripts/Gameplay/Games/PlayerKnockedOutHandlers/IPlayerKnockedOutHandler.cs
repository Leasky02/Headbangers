using System.Collections;
using UnityEngine;

public class IPlayerKnockedOutHandler
{

    public void InitOnSceneLoad()
    {
    }

    public virtual void HandleKnockedOut(Player player, GameObject knockedOutBy = null)
    {
        if (player.IsKnockedOut())
            return;

        KnockOut(player, knockedOutBy);

        float knockoutTime = player.GetComponentInChildren<PlayerKO>().GetKnockOutTime(); // TODO: improve

        player.StartCoroutine(RevivePlayerIn(player, knockoutTime));
    }

    private void KnockOut(Player player, GameObject knockedOutBy = null)
    {
        player.SetKnockedOut(true);

        player.GetComponent<PlayerFace>().KnockedOut();

        player.GetComponent<PlayerAudio>().PlayKnockOutSound();

        if (knockedOutBy)
        {
            player.GetComponent<PlayerBody>().KnockBack(knockedOutBy.transform.position, true);
        }

        player.GetComponent<PlayerBody>().KnockOut();
    }

    private IEnumerator RevivePlayerIn(Player player, float reviveIn)
    {
        yield return new WaitForSeconds(reviveIn);

        player.SetKnockedOut(false);

        player.GetComponent<PlayerHealth>().Recover();

        player.GetComponent<PlayerFace>().Revived();

        player.GetComponent<PlayerBody>().Revive();

        player.StartCoroutine(player.GetComponentInChildren<PlayerKO>().ImproveEndurance()); // TODO: ask Alasdair, does this need stopped at some point i.e. when knocked out

        float graceTime = player.GetComponentInChildren<PlayerKO>().GetGraceTime(); // TODO: improve

        yield return new WaitForSeconds(graceTime);
        player.EnableBeingHeadbutted();
    }
}
