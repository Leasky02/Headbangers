using System.Collections;
using UnityEngine;

// TODO: Add IPlayerDeathHandler script

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private BoxCollider[] bodyDetectors;
    [SerializeField] private GameObject[] bodyParts;
    [SerializeField] private MeshRenderer[] shoes;
    [SerializeField] private BoxCollider hipCollider;

    //player dies
    public IEnumerator Die()
    {
        Player player = GetComponent<Player>();

        player.SetDead(true);

        PlayerConfigurationManager.Instance.SwitchCurrentActionMap(player.GetPlayerIndex(), "Deactive");
        player.SetIsCameraTarget(false);
        Respawn();

        player.GetComponent<PlayerAudio>().PlayDeadSound();

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

        //remove body tag
        bodyParts[0].tag = "Untagged";
        bodyParts[3].tag = "Untagged";

        //legLimbs[0].GetComponent<LimbReplicator>().enabled = false;
        //legLimbs[1].GetComponent<LimbReplicator>().enabled = false;
        //legLimbs[2].GetComponent<LimbReplicator>().enabled = false;
        // legLimbs[3].GetComponent<LimbReplicator>().enabled = false;

        //bodyParts[1].GetComponent<LimbReplicator>().enabled = false;
        //bodyParts[2].GetComponent<LimbReplicator>().enabled = false;

        //foreach (GameObject legPart in legParts)
        //{
        //    legPart.layer = LayerMask.NameToLayer("PlayerLegs_Ghost");
        //    legPart.GetComponent<BoxCollider>().enabled = false;
        //}

        //disable feet colliders
        //bodyParts[1].GetComponent<MeshCollider>().enabled = false;
        //bodyParts[2].GetComponent<MeshCollider>().enabled = false;

        //hide feet
        shoes[0].enabled = false;
        shoes[1].enabled = false;

        //enable hip collider
        //hipCollider.enabled = true;

        //change material of body and face
        GetComponent<PlayerColor>().UpdateMaterial();
    }

    //player respawns
    private void Respawn()
    {
        Gameplay.Instance.PlayerRespawn.RespawnPlayer(GetComponent<Player>());
    }

    //when player is brought back to life
    public void Revive()
    {
        Respawn();

                //disable the bodyDetector
        foreach (BoxCollider boxes in bodyDetectors)
        {
            boxes.gameObject.GetComponent<BodyDetector>().enabled = true;
        }

        //change layers of body parts
        foreach (GameObject bodyPart in bodyParts)
        {
            bodyPart.layer = LayerMask.NameToLayer("Player");
        }

        //remove body tag
        bodyParts[0].tag = "Body";
        bodyParts[3].tag = "Body";

        //change material of body and face
        GetComponent<PlayerColor>().UpdateMaterial();
    }
}
