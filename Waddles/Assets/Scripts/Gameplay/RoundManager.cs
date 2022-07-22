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
        // TODO: Why is this set to false? Could it be set to false at the start of a new lobby or the end of the game?
        PlayerConfigurationManager.Instance.SetAllPlayerReady(false);

        SceneManager.LoadScene(sceneToLoad);
        StartCoroutine(GameCountdown());
    }

    public IEnumerator GameCountdown()
    {
        PlayerConfigurationManager.Instance.SwitchCurrentActionMap("Deactive");

        yield return new WaitForSeconds(1f);
        countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1f);
        countdownText.text = "Fight!";

        PlayerConfigurationManager.Instance.SwitchCurrentActionMap("Gameplay");

        yield return new WaitForSeconds(1.5f);
        countdownText.text = "";
    }
}
