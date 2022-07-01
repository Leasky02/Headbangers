using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private PlayerColor playerColor;
    private Transform spawnPoint;

    [SerializeField] private BoxCollider[] bodyDetectors; 

    [SerializeField] private GameObject[] bodyParts;
    [SerializeField] private MeshRenderer[] shoes;
    [SerializeField] private GameObject[] legParts;

    private void Start()
    {
        //set spawn point
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
    }

    //player dies
    public void Die()
    {
        Respawn();

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
    private void Respawn()
    {
        Vector3 newPosition = new Vector3(spawnPoint.position.x + (Random.Range(-1f, 1f)), spawnPoint.position.y, spawnPoint.position.z + (Random.Range(-1f, 1f)));
        transform.position = newPosition;
    }

    //when player is brought back to life
    public void Revive()
    {
        Respawn();

        //enable the bodyDetector
        foreach (BoxCollider boxes in bodyDetectors)
        {
            boxes.gameObject.GetComponent<BodyDetector>().enabled = true;
        }

        //change layers of body parts
        foreach (GameObject bodyPart in bodyParts)
        {
            bodyPart.layer = LayerMask.NameToLayer("Player");
        }

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
}
