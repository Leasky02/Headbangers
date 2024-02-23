using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    //list of all windows in the scene
    [SerializeField] private GameObject[] windows;

    private string dayPhase = "day";

    [SerializeField] private float minWindowOnDelay;
    [SerializeField] private float maxWindowOnDelay;

    [SerializeField] private float minWindowOffDelay;
    [SerializeField] private float maxWindowOffDelay;

    [SerializeField] private int switchOnPercent;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("FindWindows", 0.1f);
    }

    private void FindWindows()
    {
        windows = GameObject.FindGameObjectsWithTag("Window");
    }

    // Update is called once per frame
    void Update()
    {
        //if evening has started
        if (Clock.Instance.GetDaylightPhase() == "Evening" && dayPhase != "night")
        {
            dayPhase = "night";

            //turn window on
            foreach (GameObject window in windows)
            {
                //determine if window should turn on
                if (Random.Range(0, 100) < switchOnPercent)
                {
                    Window myWin = window.GetComponent<Window>();

                    // Generate a random value between min and max
                    float randomValue = Random.Range(minWindowOnDelay, maxWindowOnDelay);

                    // Use the triangular distribution to get the probability
                    float probability = TriangularDistribution(randomValue, minWindowOnDelay, maxWindowOnDelay, (minWindowOnDelay + maxWindowOnDelay) / 2f);

                    // Interpolate between min and max based on the probability
                    float randomizedDelay = Mathf.Lerp(minWindowOnDelay, maxWindowOnDelay, probability);

                    myWin.InvokeTurnOn(randomizedDelay);
                }
            }
        }
        //if night has started
        else if (Clock.Instance.GetDaylightPhase() == "Night" && dayPhase != "day")
        {
            dayPhase = "day";

            //turn window off
            foreach (GameObject window in windows)
            {
                Window myWin = window.GetComponent<Window>();

                //if window is on
                if (myWin.GetIsOn())
                {
                    //turn off
                    float randomValue = Random.Range(minWindowOffDelay, maxWindowOffDelay);

                    // Use the triangular distribution to get the probability
                    float probability = TriangularDistribution(randomValue, minWindowOffDelay, maxWindowOffDelay, (minWindowOffDelay + maxWindowOffDelay) / 2f);

                    // Interpolate between min and max based on the probability
                    float randomizedDelay = Mathf.Lerp(minWindowOffDelay, maxWindowOffDelay, probability);

                    myWin.InvokeTurnOff(randomizedDelay);
                }
            }
        }
    }

    private float TriangularDistribution(float x, float min, float max, float peak)
    {
        // Ensure x is within the specified range
        x = Mathf.Clamp(x, min, max);

        // The probability decreases linearly as x moves away from the peak
        float probability = 1 - Mathf.Abs(x - peak) / Mathf.Max(Mathf.Abs(min - peak), Mathf.Abs(max - peak));

        // Ensure probability is within [0, 1]
        return Mathf.Clamp01(probability);
    }
}
