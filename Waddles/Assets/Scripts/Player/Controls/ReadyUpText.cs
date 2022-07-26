using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyUpText : MonoBehaviour
{
    [SerializeField] private int ID;
    [SerializeField] private TextMesh readyUpText;

    // Update is called once per frame
    void Update()
    {
        if (PlayerConfigurationManager.Instance.playerConfigs.Count >= ID)
        {
            PlayerConfiguration correspondingPlayer = PlayerConfigurationManager.Instance.playerConfigs[ID-1];
            if (correspondingPlayer.IsReady)
            {
                readyUpText.text = "Ready";
            }
            else
            {
                readyUpText.text = "";
            }
        }
        else
        {
            readyUpText.text = "";
        }
    }
}
