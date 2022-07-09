using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private TMP_Text countdownText;
    bool countdownInProgress = false;
    bool inLobby = true;

    Coroutine latestCoroutine = null;

    public IEnumerator StartCountdown()
    {
        countdownInProgress = true;
        countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1f);
        countdownText.text = "";

        StartGame();
        countdownInProgress = false;
    }
    public void StopCountdown()
    {
        countdownInProgress = false;
        if (latestCoroutine != null)
            StopCoroutine(latestCoroutine);
        countdownText.text = "";
    }

    public void PlayersReady()
    {
        latestCoroutine = StartCoroutine(StartCountdown());
    }

    public bool GetCountdownInProgress()
    {
        return countdownInProgress;
    }
    public bool GetInLobby()
    {
        return inLobby;
    }

    private void StartGame()
    {
        inLobby = false;
        foreach (PlayerConfiguration config in PlayerManager.Instance.playerConfigs)
        {
            config.IsReady = false;
        }

        SceneManager.LoadScene(sceneToLoad);
        StartCoroutine(GameCountdown());
    }

    public IEnumerator GameCountdown()
    {
        foreach(PlayerConfiguration config in PlayerManager.Instance.playerConfigs)
        {
            config.Input.SwitchCurrentActionMap("Deactive");
        }

        yield return new WaitForSeconds(1f);
        countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1f);
        countdownText.text = "Fight!";

        foreach (PlayerConfiguration config in PlayerManager.Instance.playerConfigs)
        {
            config.Input.SwitchCurrentActionMap("Gameplay");
        }

        yield return new WaitForSeconds(1.5f);
        countdownText.text = "";
    }
}
