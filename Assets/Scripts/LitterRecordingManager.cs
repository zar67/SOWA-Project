using Mapbox.Examples;
using Mapbox.Unity.Location;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LitterRecordingManager : SingletonMonoBehaviour<LitterRecordingManager>
{
    public static event Action OnRecordedLitterUpdated;

    public const string LITTER_KEY = "LITTER_DATA";

    [SerializeField] private float m_mergeDistance = 0.0001f;

    private AbstractLocationProvider m_locationProvider = null;

    public List<LitterData> StoredLitterData { get; private set; }  = new List<LitterData>();

    private void OnEnable()
    {
        FirebaseDatabaseManager.Instance.AddValueChangedListener(LITTER_KEY, HandleLitterDatabaseUpdated);

        // Clear old litter data here?
    }

    private void OnDisable()
    {
        FirebaseDatabaseManager.Instance.RemoveValueChangedListener(LITTER_KEY, HandleLitterDatabaseUpdated);
    }

    public void RecordLitter()
    {
        if (m_locationProvider == null)
        {
            m_locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider as AbstractLocationProvider;
        }

        Location currentLocation = m_locationProvider.CurrentLocation;

        var data = new LitterData()
        {
            Timestamp = DateTime.UtcNow.ToString(),
            Location = $"{currentLocation.LatitudeLongitude.x},{currentLocation.LatitudeLongitude.y}"
        };

        FirebaseDatabaseManager.Instance.AppendData(LITTER_KEY, data);
    }

    private void HandleLitterDatabaseUpdated(object data, Firebase.Database.ValueChangedEventArgs args)
    {
        var dataDict = args.Snapshot.Value as Dictionary<string, object>;
        StoredLitterData = new List<LitterData>();

        var distanceCheckList = new List<object>(dataDict.Values);

        for (int i = 0; i < distanceCheckList.Count; i++)
        {
            LitterData litterData = JsonUtility.FromJson<LitterData>(distanceCheckList[i] as string);
            Vector2d location = Conversions.StringToLatLon(litterData.Location);

            bool merged = false;
            if (i < distanceCheckList.Count - 1)
            {
                for (int j = i + 1; j < distanceCheckList.Count; j++)
                {
                    LitterData compareLitterData = JsonUtility.FromJson<LitterData>(distanceCheckList[j] as string);
                    Vector2d compareLocation = Conversions.StringToLatLon(compareLitterData.Location);
                    if (Vector2d.Distance(location, compareLocation) < m_mergeDistance)
                    {
                        merged = true;
                        distanceCheckList[j] = JsonUtility.ToJson(litterData.Merge(compareLitterData));
                    }
                }
            }

            if (!merged)
            {
                StoredLitterData.Add(litterData);
            }
        }
    }
}
