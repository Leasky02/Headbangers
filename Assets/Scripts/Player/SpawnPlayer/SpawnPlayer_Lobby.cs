using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnPlayer_Lobby : LS.ISpawnPlayer<PlayerConfiguration>
{
    //spawnpositions
    [SerializeField] private LobbySpawnPoint[] spawnPositions;

    public override void SpawnPlayer(PlayerConfiguration playerConfig)
    {
        int playerIndex = playerConfig.PlayerIndex;
        PlayerInput playerInput = playerConfig.Input;

        playerInput.transform.GetComponent<Player>().Init(playerIndex);

        SetPosition(playerConfig);
        SetInputMap(playerConfig);
        FreezePosition(playerConfig);
    }

    public override void ShufflePlayer(PlayerConfiguration playerConfig)
    {
        int userIndex = playerConfig.GetUserIndex();
        Debug.Log(userIndex);
        //spawnPositions[userIndex].SetReadyText(false);

        SetPosition(playerConfig);
        SetInputMap(playerConfig);
    }

    public void SetPosition(PlayerConfiguration playerConfig)
    {
        Transform playerTransform = playerConfig.Input.transform.GetChild(0);
        playerTransform.position = spawnPositions[playerConfig.GetUserIndex()].GetPosition();
    }

    public void FreezePosition(PlayerConfiguration playerConfig)
    {
        Rigidbody playerHip = playerConfig.Input.gameObject.transform.GetChild(0).GetComponent<Rigidbody>();
        playerHip.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void SetInputMap(PlayerConfiguration playerConfig)
    {
        playerConfig.Input.SwitchCurrentActionMap("Lobby");
    }
}
