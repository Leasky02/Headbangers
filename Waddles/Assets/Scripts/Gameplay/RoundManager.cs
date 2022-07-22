using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RoundManager : Singleton<RoundManager>
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private TMP_Text countdownText;
    bool countdownInProgress = false;

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

    public bool IsCountdownInProgress()
    {
        return countdownInProgress;
    }

    private void StartGame()
    {
        // TODO: Why is this set to false? Could it be set to false at the start of a new lobby or the end of the game?
        PlayerConfigurationManager.Instance.SetAllPlayerReady(false);

        PlayerConfigurationManager.Instance.DisableJoining();
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
