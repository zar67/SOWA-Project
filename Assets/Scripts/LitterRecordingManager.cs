using Mapbox.Unity.Location;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LitterRecordingManager : SingletonMonoBehaviour<LitterRecordingManager>
{
    public static event Action OnRecordedLitterUpdated;

    public const string LITTER_KEY = "LITTER_DATA";

    private AbstractLocationProvider m_locationProvider = null;

    public List<LitterData> StoredLitterData { get; private set; }  = new List<LitterData>();

    private void OnEnable()
    {
        FirebaseDatabaseManager.Instance.AddValueChangedListener(LITTER_KEY, HandleLitterDatabaseUpdated);
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
            Location = currentLocation.LatitudeLongitude.ToString()
        };

        FirebaseDatabaseManager.Instance.AppendData(LITTER_KEY, data);
    }

    private void HandleLitterDatabaseUpdated(object data, Firebase.Database.ValueChangedEventArgs args)
    {
        var dataDict = args.Snapshot.Value as Dictionary<string, object>;
        StoredLitterData = new List<LitterData>();
        foreach (object litterData in dataDict.Values)
        {
            StoredLitterData.Add(JsonUtility.FromJson<LitterData>(litterData as string));
        }
    }
}
