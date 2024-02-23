using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : Singleton<Clock>
{
    [Tooltip("Length of each daylight cycle in seconds.")]
    [SerializeField] private float cycleDuration;

    private int daysElapsed = 0;
    private string daylightPhase = "Morning";

    private void Update()
    {
        CalculateTime();
    }

    public void CalculateTime()
    {
        //caluclates day phase and days elapsed

        //PUT THIS INTO 24 HOUR CLOCK instead of percentage??
        //get daylight cycle progression as a percentage
        float dayCycleCompletion = ((Time.time % cycleDuration) / cycleDuration) * 100;

        //If it is morning: 0-25% of the day (phase 1)
        if ((dayCycleCompletion >= 0f && dayCycleCompletion < 25f) && daylightPhase != "Morning")
        {
            Debug.Log("Morning has arrived");
            daylightPhase = "Morning";

            IncrementDaysElapsed();
        }
        //If it is day: 25-50% of the day (phase 2)
        else if ((dayCycleCompletion >= 25f && dayCycleCompletion < 50f) && daylightPhase != "Day")
        {
            Debug.Log("It is midday");
            daylightPhase = "Day";
        }
        //If it is evening: 50-75% of the day (phase 3)
        else if ((dayCycleCompletion >= 50f && dayCycleCompletion < 75f) && daylightPhase != "Evening")
        {
            Debug.Log("Evening has arrived");
            daylightPhase = "Evening";
        }
        //If it is night: 75-100% of the day (phase 4)
        else if ((dayCycleCompletion >= 75f && dayCycleCompletion < 100f) && daylightPhase != "Night")
        {
            Debug.Log("It is midnight");
            daylightPhase = "Night";
        }

    }

    public void IncrementDaysElapsed()
    {
        daysElapsed++;

        Debug.Log("Days elapsed: " + daysElapsed);
    }

    public string GetDaylightPhase()
    {
        return daylightPhase;
    }

    public int GetDaysElapsed()
    {
        return daysElapsed;
    }

    public float GetCycleDuration()
    {
        return cycleDuration;
    }
}
