using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayCycleLighting : MonoBehaviour
{
    [Header("Light Sources")]
    //Light sources
    private Light globalLight;

    [Header("Global Light Sine Wave")]
    //Total range of global light Sine wave
    [Tooltip("The value above and below zero that the sin wave reaches for global light.")]
    [SerializeField] private float globalLightRange;
    //Min and max values desired in global light source
    [Tooltip("The minimum value in the range for the global light. (Difference between min and max == range * 2)")]
    [SerializeField] private float globalLightMin;
    [Tooltip("The maximum value in the range for the global light. (Difference between min and max == range * 2)")]
    [SerializeField] private float globalLightMax;

    [Header("Global Light Colour Sine Wave")]
    //Total range of global light colour Sine wave
    [Tooltip("The value above and below zero that the sin wave reaches for global light colour.")]
    [SerializeField] private float globalColourRange;
    //Min and max values of global light
    [Tooltip("The minimum value in the range for the global light colour. (Difference between min and max == range * 2)")]
    [SerializeField] private float globalColourMin;
    [Tooltip("The maximum value in the range for the global light colour. (Difference between min and max == range * 2)")]
    [SerializeField] private float globalColourMax;
    //Colour values of global colour to lerp between
    [Tooltip("The colour value in the range of global light colour. This is shown during sunrise and sunset.")]
    [SerializeField] private Color colourRangeSun;
    [Tooltip("The colour value in the range of global light colour. This is shown during the night.")]
    [SerializeField] private Color colourRangeNight;

    [Header("Global Light Saturation Sine Wave")]
    //Total range of global light saturation Sine wave
    [Tooltip("The value above and below zero that the sin wave reaches for global light saturation.")]
    [SerializeField] private float globalSaturationRange;
    //Min and max values of global light saturation
    [Tooltip("The minimum value in the range for the global light saturation. (Difference between min and max == range * 2)")]
    [SerializeField] private float globalSaturationMin;
    [Tooltip("The maximum value in the range for the global light saturation. (Difference between min and max == range * 2)")]
    [SerializeField] private float globalSaturationMax;
    //Min and max values of global light saturation
    [Tooltip("0-1: The minimum value in the range of global light saturation. This is shown during the day.")]
    [SerializeField] private float saturationRangeDay;
    [Tooltip("0-1: The maximum value in the range of global light saturation. This is shown during the night.")]
    [SerializeField] private float saturationRangeNight;

    [Header("Fog Colours")]
    //Colour values of global colour to lerp between
    [Tooltip("The colour value in the range of global light colour. This is shown during sunrise and sunset.")]
    [SerializeField] private Color fogColourRangeSun;
    [Tooltip("The colour value in the range of global light colour. This is shown during the night.")]
    [SerializeField] private Color fogColourRangeNight;


    void Start()
    {
        globalLight = GetComponent <Light>();
    }

    private void FixedUpdate()
    {
        SetGlobalLight();
        SetGlobalColour();
        SetFogColour();

        //put in window manager
        //cycleTime.CalculateTimeElapsed();
    }

    private void SetGlobalLight()
    {
        //get global light intensity
        float globalLightIntensity = CalculateGlobalLightSineWave();

        //cap it to 1
        if (globalLightIntensity > 1)
            globalLightIntensity = 1;

        //set global light intensity
        globalLight.intensity = globalLightIntensity;
    }

    private void SetGlobalColour()
    {
        //get Sine value for colour
        float globalColourLevel = CalculateColourSineWave();
        //get Sine value for saturation
        float globalsaturationLevel = CalculateSaturationSineWave();

        //create a basic colour
        Color color = Color.white;

        //calculate the midpoint in the Sine wave
        float midwayPoint = (globalColourMin + globalColourMax) * 0.5f;

        // Calculate the current colour based on the time of day
        if (globalColourLevel < midwayPoint)
        {
            float lerpRange = midwayPoint;
            float adjustedGlobalColourLevel = (globalColourLevel) / lerpRange;
            color = colourRangeSun;
        }
        else
        {
            float lerpRange = 1.0f - midwayPoint;
            float adjustedGlobalColourLevel = (globalColourLevel - midwayPoint) / lerpRange;
            color = Color.Lerp(colourRangeSun, colourRangeNight, adjustedGlobalColourLevel);
        }

        //convert colour to HSV
        float h, s, v;
        Color.RGBToHSV(color, out h, out s, out v);

        // Calculate the current saturation based on the time of day
        s = Mathf.Lerp(saturationRangeDay, saturationRangeNight, globalsaturationLevel);

        //convert back toRGB with saturation required
        color = Color.HSVToRGB(h, s, v);

        globalLight.color = color;
    }

    private void SetFogColour()
    {
        //use sine wave of light
        float fogColourLevel = CalculateFogSineWave();

        //create a basic colour
        Color color = Color.white;

        //calculate the midpoint in the Sine wave
        float midwayPoint = (globalColourMin + globalColourMax) * 0.5f;

        // Calculate the current colour based on the time of day
        if (fogColourLevel < midwayPoint)
        {
            float lerpRange = midwayPoint;
            float adjustedGlobalColourLevel = (fogColourLevel) / lerpRange;
            color = fogColourRangeSun;
        }
        else
        {
            float lerpRange = 1.0f - midwayPoint;
            float adjustedGlobalColourLevel = (fogColourLevel - midwayPoint) / lerpRange;
            color = Color.Lerp(fogColourRangeSun, fogColourRangeNight, adjustedGlobalColourLevel);
        }

        // Modify the fog color in RenderSettings
        RenderSettings.fogColor = color;
    }

    private float CalculateGlobalLightSineWave()
    {
        float globalLightLevel = Mathf.Sin((Mathf.PI * 2.0f * Time.time) / Clock.Instance.GetCycleDuration());
        globalLightLevel *= globalLightRange;
        globalLightLevel += (globalLightMin + globalLightMax) * 0.5f;
        globalLightLevel = Mathf.Round(globalLightLevel * 1000) * 0.001f;

        return globalLightLevel;
    }

    private float CalculateFogSineWave()
    {
        float offsetDegreesColor = 180f;
        float offsetRadiansColor = offsetDegreesColor * Mathf.Deg2Rad;
        float globalLightLevel = Mathf.Sin(((Mathf.PI * 2.0f * Time.time) / Clock.Instance.GetCycleDuration()) + offsetRadiansColor);

        globalLightLevel *= globalLightRange;
        globalLightLevel += (globalLightMin + globalLightMax) * 0.5f;
        globalLightLevel = Mathf.Round(globalLightLevel * 1000) * 0.001f;

        return globalLightLevel;
    }

    private float CalculateColourSineWave()
    {
        float offsetDegreesColor = 180f;
        float offsetRadiansColor = offsetDegreesColor * Mathf.Deg2Rad;
        float colour = Mathf.Sin(((Mathf.PI * 2.0f * Time.time) / Clock.Instance.GetCycleDuration()) + offsetRadiansColor);

        colour *= globalColourRange;
        colour += (globalColourMin + globalColourMax) * 0.5f;
        colour = Mathf.Round(colour * 1000) * 0.001f;

        return colour;
    }

    private float CalculateSaturationSineWave()
    {
        float offsetDegrees = 180f;
        float offsetRadians = offsetDegrees * Mathf.Deg2Rad;
        float sat = Mathf.Sin(((2.0f * Mathf.PI * Time.time) / Clock.Instance.GetCycleDuration()) + offsetRadians);

        sat *= globalSaturationRange;
        sat += (globalSaturationMin + globalSaturationMax) * 0.5f;
        sat = Mathf.Round(sat * 1000) * 0.001f;

        return sat;
    }
}