using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class for determining the location of the user's device.
/// </summary>
public class GPS : MonoBehaviour
{
    private const int MAX_INIT_WAIT = 20;

    /// <summary>
    /// Static instance.
    /// </summary>
    public static GPS Instance
    {
        get; private set;
    }

    /// <summary>
    /// Lastest longitude of the device's location.
    /// </summary>
    public double Longitude => Input.location.lastData.longitude;

    /// <summary>
    /// Lastest latitude of the device's location.
    /// </summary>
    public double Latitude => Input.location.lastData.latitude;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning($"Cannot have two instances of GPS class, not initialising GPS on {gameObject.name}");
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
        StartCoroutine(InitializeLocationService());
    }

    private IEnumerator InitializeLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("Input has not been enabled by the user, cannot initialise GPS");
            yield break;
        }

        Input.location.Start();

        float waitTimer = 0f;
        while (Input.location.status == LocationServiceStatus.Initializing || waitTimer <= MAX_INIT_WAIT)
        {
            yield return new WaitForSeconds(1);
            waitTimer++;
        }

        if (waitTimer >= MAX_INIT_WAIT)
        {
            Debug.LogError("Initializing location timed out, GPS not initialized");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Initializing location failed, GPS not initialized");
            yield break;
        }
    }
}
