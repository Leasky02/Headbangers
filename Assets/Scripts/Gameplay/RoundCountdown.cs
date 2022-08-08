using System.Collections;
using TMPro;
using UnityEngine;

// TODO: rename to round manager
public class RoundCountdown : MonoBehaviour
{
    [SerializeField] private TMP_Text countdownText;

    public void Start()
    {
        StartRoundCountdown();
    }

    private void StartRoundCountdown()
    {
        StartCoroutine(GameCountdown());
    }

    private IEnumerator GameCountdown()
    {
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
