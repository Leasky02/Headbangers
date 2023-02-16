using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyCountdown : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    [SerializeField] private TMP_Text countdownText;

    bool m_countdownInProgress = false;

    Coroutine latestCoroutine = null;

    private IEnumerator StartCountdown()
    {
        m_countdownInProgress = true;
        countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1f);
        countdownText.text = "";

        //delay for faded exit
        FadeExit();

        yield return new WaitForSeconds(0.75f);

        StartGame();
        m_countdownInProgress = false;
    }
    public void StopCountdown()
    {
        m_countdownInProgress = false;
        if (latestCoroutine != null)
        {
            StopCoroutine(latestCoroutine);
        }
        countdownText.text = "";
    }

    public void PlayersReady()
    {
        latestCoroutine = StartCoroutine(StartCountdown());
    }

    public bool IsCountdownInProgress()
    {
        return m_countdownInProgress;
    }

    private void StartGame()
    {
        PlayerConfigurationManager.Instance.DisableJoining();
        PlayerConfigurationManager.Instance.SwitchCurrentActionMap("Gameplay");

        SceneManager.LoadScene(sceneToLoad);
    }

    private void FadeExit()
    {
        //queue screen fade

        MusicManager.Instance.StopTrack();
    }
}
