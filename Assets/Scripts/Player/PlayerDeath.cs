using System.Collections;
using UnityEngine;

// TODO: Add IPlayerDeathHandler script

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private BoxCollider[] bodyDetectors;
    [SerializeField] private GameObject[] bodyParts;
    [SerializeField] private MeshRenderer[] shoes;
    [SerializeField] private GameObject[] legParts;
    [SerializeField] private GameObject[] legLimbs;
    [SerializeField] private BoxCollider hipCollider;

    //player dies
    public IEnumerator Die()
    {
        Player.GetPlayerComponent(gameObject).SetDead(true);

        PlayerConfigurationManager.Instance.SwitchCurrentActionMap(Player.GetPlayerComponent(gameObject).GetPlayerIndex(), "Deactive");
        Player.GetPlayerComponent(gameObject).SetIsCameraTarget(false);
        Respawn();

        Player.GetPlayerComponent(gameObject).GetComponent<PlayerAudio>().PlayDeadSound();

        yield return new WaitForSeconds(1f);

        //disable the bodyDetector
        foreach (BoxCollider boxes in bodyDetectors)
        {
            boxes.gameObject.GetComponent<BodyDetector>().enabled = false;
        }

        //change layers of body parts
        foreach (GameObject bodyPart in bodyParts)
        {
            bodyPart.layer = LayerMask.NameToLayer("Player_Ghost");
        }

        //disable animations on legs
        legLimbs[0].GetComponent<LimbReplicator>().enabled = false;
        legLimbs[1].GetComponent<LimbReplicator>().enabled = false;
        legLimbs[2].GetComponent<LimbReplicator>().enabled = false;
        legLimbs[3].GetComponent<LimbReplicator>().enabled = false;

        bodyParts[1].GetComponent<LimbReplicator>().enabled = false;
        bodyParts[2].GetComponent<LimbReplicator>().enabled = false;

        foreach (GameObject legPart in legParts)
        {
            legPart.layer = LayerMask.NameToLayer("PlayerLegs_Ghost");
            legPart.GetComponent<BoxCollider>().enabled = false;
        }

        //remove body tag
        bodyParts[0].tag = "Untagged";
        bodyParts[3].tag = "Untagged";

        //disable feet colliders
        bodyParts[1].GetComponent<MeshCollider>().enabled = false;
        bodyParts[2].GetComponent<MeshCollider>().enabled = false;

        //hide feet
        shoes[0].enabled = false;
        shoes[1].enabled = false;

        //enable hip collider
        hipCollider.enabled = true;

        //change material of body and face
        GetComponent<PlayerColor>().UpdateMaterial();
    }

    //player respawns
    private void Respawn()
    {
        Gameplay.Instance.PlayerRespawn.RespawnPlayer(Player.GetPlayerComponent(gameObject));
    }

    //when player is brought back to life
    public void Revive()
    {
        Respawn();

        //change layers of body parts
        foreach (GameObject bodyPart in bodyParts)
        {
            bodyPart.layer = LayerMask.NameToLayer("Player");
        }

        //enable animations on legs
        bodyParts[1].GetComponent<LimbReplicator>().enabled = true;
        bodyParts[2].GetComponent<LimbReplicator>().enabled = true;

        //enable animations on legs
        legLimbs[0].GetComponent<LimbReplicator>().enabled = true;
        legLimbs[1].GetComponent<LimbReplicator>().enabled = true;
        legLimbs[2].GetComponent<LimbReplicator>().enabled = true;
        legLimbs[3].GetComponent<LimbReplicator>().enabled = true;

        foreach (GameObject legPart in legParts)
        {
            legPart.layer = LayerMask.NameToLayer("PlayerLegs");
            legPart.GetComponent<BoxCollider>().enabled = true;

        }

        //remove body tag
        bodyParts[0].tag = "Body";

        //show feet
        shoes[0].enabled = true;
        shoes[1].enabled = true;

        //disable feet colliders
        bodyParts[1].GetComponent<MeshCollider>().enabled = false;
        bodyParts[2].GetComponent<MeshCollider>().enabled = false;

        //disable hip collider
        hipCollider.enabled = false;

        //change material of body and face
        GetComponent<PlayerColor>().UpdateMaterial();
    }
}
