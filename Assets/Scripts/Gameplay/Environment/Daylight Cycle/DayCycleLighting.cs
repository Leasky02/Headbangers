using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayCycleLighting : MonoBehaviour
{
    [Header("Light Sources")]
    //Light sources
    private Light globalLight;
    [SerializeField] private Light[] levelLights;

    [Header("Global Light Sine Wave")]
    //minimum light level
    [SerializeField] private float minLight = 0.05f;
    //minimum light level
    [SerializeField] private float maxLight = 1f;

    //Total range of global light Sine wave
    [Tooltip("The value above and below zero that the sin wave reaches for global light.")]
    [SerializeField] private float globalLightRange;
    //Min and max values desired in global light source
    [Tooltip("The minimum value in the range for the global light. (Difference between min and max == range * 2)")]
    [SerializeField] private float globalLightMin;
    [Tooltip("The maximum value in the range for the global light. (Difference between min and max == range * 2)")]
    [SerializeField] private float globalLightMax;

    [Header("Level Light Sine Wave")]

    //Total range of global light Sine wave
    [Tooltip("The value above and below zero that the sin wave reaches for level light.")]
    [SerializeField] private float levelLightRange;
    //Min and max values desired in global light source
    [Tooltip("The minimum value in the range for the level light. (Difference between min and max == range * 2)")]
    [SerializeField] private float levelLightMin;
    [Tooltip("The maximum value in the range for the level light. (Difference between min and max == range * 2)")]
    [SerializeField] private float levelLightMax;

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

    [Header("Fog Colour Sine Wave")]
    //Total range of global light Sine wave
    [Tooltip("The value above and below zero that the sin wave reaches for level light.")]
    [SerializeField] private float fogColourRange;
    //Min and max values desired in global light source
    [Tooltip("The minimum value in the range for the level light. (Difference between min and max == range * 2)")]
    [SerializeField] private float fogColourMin;
    [Tooltip("The maximum value in the range for the level light. (Difference between min and max == range * 2)")]
    [SerializeField] private float fogColourMax;

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
        SetLevelLight();
        SetGlobalColour();
        SetFogColour();
    }

    private void SetGlobalLight()
    {
        //get global light intensity
        float globalLightIntensity = CalculateGlobalLightSineWave();

        globalLightIntensity = Mathf.Clamp(globalLightIntensity, minLight, maxLight);

        //set global light intensity
        globalLight.intensity = globalLightIntensity;
    }
    private void SetLevelLight()
    {
        //get global light intensity
        float levelLightIntensity = CalculateLevelLightSineWave();

        levelLightIntensity = Mathf.Clamp(levelLightIntensity, 0f, 1f);

        //set global light intensity
        foreach(Light light in levelLights)
        {
            light.intensity = levelLightIntensity;
        }
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
        float fogColourLevel = CalculateFogColourSineWave();

        //create a basic colour
        Color color = Color.white;

        //calculate the midpoint in the Sine wave
        float midwayPoint = (globalColourMin + globalColourMax) * 0.5f;

        // Calculate the current colour based on the time of day
        if (fogColourLevel < midwayPoint)
        {
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

    private float CalculateLevelLightSineWave()
    {
        float offsetDegreesColor = 180f;
        float offsetRadiansColor = offsetDegreesColor * Mathf.Deg2Rad;
        float levelLightLevel = Mathf.Sin(((Mathf.PI * 2.0f * Time.time) / Clock.Instance.GetCycleDuration()) + offsetRadiansColor);

        levelLightLevel *= levelLightRange;
        levelLightLevel += (levelLightMin + levelLightMax) * 0.5f;
        levelLightLevel = Mathf.Round(levelLightLevel * 1000) * 0.001f;

        return levelLightLevel;
    }

    private float CalculateFogColourSineWave()
    {
        float offsetDegreesColor = 180f;
        float offsetRadiansColor = offsetDegreesColor * Mathf.Deg2Rad;
        float fogColourLevel = Mathf.Sin(((Mathf.PI * 2.0f * Time.time) / Clock.Instance.GetCycleDuration()) + offsetRadiansColor);

        fogColourLevel *= fogColourRange;
        fogColourLevel += (fogColourMin + fogColourMax) * 0.5f;
        fogColourLevel = Mathf.Round(fogColourLevel * 1000) * 0.001f;

        return fogColourLevel;
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