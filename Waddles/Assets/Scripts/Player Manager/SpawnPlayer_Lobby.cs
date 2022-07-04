using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer_Lobby : ISpawnPlayer
{
    //spawnpositions
    [SerializeField] private Transform[] spawnPositions;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SpawnPlayer(PlayerConfiguration playerConfig)
    {
        Debug.Log(playerConfig.PlayerIndex);
        SetPosition(playerConfig);
        SetInputMap(playerConfig);
    }

    private void SetPosition(PlayerConfiguration playerConfig)
    {
        Transform playerTransform = playerConfig.Input.transform.GetChild(0);
        playerTransform.position = spawnPositions[playerConfig.PlayerIndex].position;
    }

    private void SetInputMap(PlayerConfiguration playerConfig)
    {
        playerConfig.Input.SwitchCurrentActionMap("UI");
    }
}
