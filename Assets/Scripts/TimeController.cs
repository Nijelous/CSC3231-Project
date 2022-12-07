using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Controls the day/night cycle in the scene, taking in a lot of variables
public class TimeController : MonoBehaviour
{
    // How fast time will run
    [SerializeField]
    private float timeMultiplier;

    // The start time on the 24 hour clock
    [SerializeField]
    private float startHour;

    // The current time in the scene
    private DateTime currentTime;

    // The two lights, being run at opposite positions to each other
    [SerializeField]
    private Light sun;

    [SerializeField]
    private Light moon;

    // The hours which day and nights switch
    private float sunriseHour = 6.5f;

    private float sunsetHour = 18f;

    private TimeSpan sunriseTime;

    private TimeSpan sunsetTime;

    // The ambient lights of daytime and nighttime
    [SerializeField]
    private Color dayAmbientLight;

    [SerializeField]
    private Color nightAmbientLight;

    // The curve which the light intensity changes over time
    [SerializeField]
    private AnimationCurve lightChangeCurve;

    // The intensity of the light at night and day
    [SerializeField]
    private float maxSunlightIntensity;

    [SerializeField]
    private float maxMoonlightIntensity;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);

        sunriseTime = TimeSpan.FromHours(sunriseHour);

        sunsetTime = TimeSpan.FromHours(sunsetHour);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeOfDay();
        RotateSun();
        UpdateLightSettings();
    }

    // Ticks through the time
    private void UpdateTimeOfDay()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);
    }

    // Moves the position of the sun and moon dependiing on what time it is, increasing the X axis value
    private void RotateSun()
    {
        float sunLightRotation;

        // If it is day, rotates between 0 and 180 (pointing down upon the scene), taking the interpolated value based on how far through the day the time is
        if(currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
        }
        // If it is night, rotates between 180 and 360 (pointing up at the scene), taking the interpolated value based on how far through the night the time is
        else
        {
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }

        // The moon is pointed at the exact opposite of the sun
        sun.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
        moon.transform.rotation = Quaternion.AngleAxis(sunLightRotation - 180, Vector3.right);
    }

    // Using the light curve, takes the vector between where the light is pointed and directly down and adjusts the intensity of the lights depending on where this lies on the curve
    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(sun.transform.forward, Vector3.down);
        sun.intensity = Mathf.Lerp(0, maxSunlightIntensity, lightChangeCurve.Evaluate(dotProduct));
        moon.intensity = Mathf.Lerp(maxMoonlightIntensity, 0, lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));
    }

    // Calculates the difference in the time for the rotation of the sun
    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        if(difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }

        return difference;
    }
}
