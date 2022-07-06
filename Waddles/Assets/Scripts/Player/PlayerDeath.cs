using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerColor playerColor;
    private Transform spawnPoint;

    [SerializeField] private BoxCollider[] bodyDetectors; 

    [SerializeField] private GameObject[] bodyParts;
    [SerializeField] private MeshRenderer[] shoes;
    [SerializeField] private GameObject[] legParts;
    [SerializeField] private GameObject[] legLimbs;

    //player dies
    public IEnumerator Die()
    {
        playerInput.SwitchCurrentActionMap("Respawn");
        playerData.SetIsCameraTarget(false);
        StartCoroutine(Respawn());

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
        }

        //remove body tag
        bodyParts[0].tag = "Untagged";

        //hide feet
        shoes[0].enabled = false;
        shoes[1].enabled = false;

        //change material of body and face
        playerColor.UpdateMaterial();
    }

    //player respawns
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);

        Vector3 newPosition = new Vector3(spawnPoint.position.x + (Random.Range(-1.5f, 1.5f)), spawnPoint.position.y, spawnPoint.position.z + (Random.Range(-1.5f, 1.5f)));
        transform.position = newPosition;
        yield return new WaitForSeconds(0.5f);

        playerData.SetIsCameraTarget(true);
        playerInput.SwitchCurrentActionMap("Gameplay");
    }

    //when player is brought back to life
    public void Revive()
    {
        StartCoroutine(Respawn());

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
        }

        //remove body tag
        bodyParts[0].tag = "Body";

        //show feet
        shoes[0].enabled = true;
        shoes[1].enabled = true;

        //change material of body and face
        playerColor.UpdateMaterial();
    }

    public void FindRespawnPoint()
    {
        //set respawn point
        spawnPoint = GameObject.FindGameObjectWithTag("RespawnPoint").transform;
    }
}
