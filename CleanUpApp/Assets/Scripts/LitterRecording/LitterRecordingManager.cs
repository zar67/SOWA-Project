using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LitterRecordingManager : SingletonMonoBehaviour<LitterRecordingManager>
{
    [Serializable]
    public struct ZoomMergeDistanceMapping
    {
        public float MaxZoom;
        public float MergeDistance;
    }

    public const string LITTER_KEY = "LITTER_DATA";

    [SerializeField] private AbstractMap m_map;
    [SerializeField] private List<ZoomMergeDistanceMapping> m_zoomMergeDistanceMappings;

    private float m_currentMergeDistance = 0.0001f;

    private AbstractLocationProvider m_locationProvider = null;

    public List<LitterData> FullLitterData { get; private set; } = new List<LitterData>();

    public List<List<LitterData>> CondensedLitterData { get; private set; }  = new List<List<LitterData>>();

    private void OnEnable()
    {
        FirebaseDatabaseManager.Instance.AddValueChangedListener(LITTER_KEY, HandleLitterDatabaseUpdated);

        // Clear old litter data here?
    }

    private void OnDisable()
    {
        FirebaseDatabaseManager.Instance.RemoveValueChangedListener(LITTER_KEY, HandleLitterDatabaseUpdated);
    }

    private void Update()
    {
        float currentZoom = m_map.Zoom;

        foreach (ZoomMergeDistanceMapping zoomMapping in m_zoomMergeDistanceMappings)
        {
            if (currentZoom <= zoomMapping.MaxZoom)
            {
                if (m_currentMergeDistance != zoomMapping.MergeDistance)
                {
                    m_currentMergeDistance = zoomMapping.MergeDistance;
                    UpdateCondensedLitterList();
                }

                break;
            }
        }
    }

    public void RecordLitter(string[] tags)
    {
        if (m_locationProvider == null)
        {
            m_locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider as AbstractLocationProvider;
        }

        Location currentLocation = m_locationProvider.CurrentLocation;

        var data = new LitterData()
        {
            Timestamp = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss"),
            Location = $"{currentLocation.LatitudeLongitude.x},{currentLocation.LatitudeLongitude.y}",
            Tags = tags
        };

        FirebaseDatabaseManager.Instance.AppendData(LITTER_KEY, data);
    }

    private void HandleLitterDatabaseUpdated(object data, Firebase.Database.ValueChangedEventArgs args)
    {
        var dataDict = args.Snapshot.Value as Dictionary<string, object>;
        FullLitterData = new List<LitterData>();
        CondensedLitterData = new List<List<LitterData>>();

        var distanceCheckList = new List<object>(dataDict.Values);

        for (int i = 0; i < distanceCheckList.Count; i++)
        {
            LitterData litterData = JsonUtility.FromJson<LitterData>(distanceCheckList[i] as string);
            Vector2d location = Conversions.StringToLatLon(litterData.Location);

            FullLitterData.Add(litterData);
        }

        UpdateCondensedLitterList();
    }

    private void UpdateCondensedLitterList()
    {
        var distanceCheckList = new List<LitterData>(FullLitterData);
        var handledLitter = new List<LitterData>();

        CondensedLitterData = new List<List<LitterData>>();
        for (int i = 0; i < distanceCheckList.Count; i++)
        {
            if (handledLitter.Contains(distanceCheckList[i]))
            {
                continue;
            }

            Vector2d location = Conversions.StringToLatLon(distanceCheckList[i].Location);

            var mergedLitter = new List<LitterData>() { distanceCheckList[i] };
            handledLitter.Add(distanceCheckList[i]);
            if (i < distanceCheckList.Count - 1)
            {
                for (int j = i + 1; j < distanceCheckList.Count; j++)
                {
                    Vector2d compareLocation = Conversions.StringToLatLon(distanceCheckList[j].Location);
                    if (Vector2d.Distance(location, compareLocation) < m_currentMergeDistance && !handledLitter.Contains(distanceCheckList[j]))
                    {
                        mergedLitter.Add(distanceCheckList[j]);
                        handledLitter.Add(distanceCheckList[j]);
                    }
                }
            }

            CondensedLitterData.Add(mergedLitter);
        }
    }
}
