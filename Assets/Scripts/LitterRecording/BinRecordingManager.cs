using Mapbox.Unity.Location;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class BinRecordingManager : SingletonMonoBehaviour<BinRecordingManager>
{
    private string m_myId = "zar67";
    private string m_accessToken = "sk.eyJ1IjoiemFyNjciLCJhIjoiY2x2YXV5b2hzMDI1dDJwcnN5aDByZHl5ZSJ9.ffdUioOHqd9oRxV97pklnw";
    private string m_datasetId = "clvbdf3kb1xk31trty0dc9x5w";
    private string m_tilesetId = "Bins";

    private AbstractLocationProvider m_locationProvider = null;

    protected override void Awake()
    {
        base.Awake();

        Mapbox.Unity.MapboxAccess.Instance.ClearAllCacheFiles();

        //StartCoroutine(ApplyDatasetToTileset());
    }

    public void RequestNewBinLocation()
    {
        if (m_locationProvider == null)
        {
            m_locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider as AbstractLocationProvider;
        }

        Location currentLocation = m_locationProvider.CurrentLocation;

        StartCoroutine(PostNewDatasetFeature(currentLocation.LatitudeLongitude.x, currentLocation.LatitudeLongitude.y));
    }

    IEnumerator PostNewDatasetFeature(double lat, double longitude)
    {
        string postURL = $"https://api.mapbox.com/datasets/v1/{m_myId}/{m_datasetId}/features/{Guid.NewGuid()}?access_token={m_accessToken}";

        string geoJSONstring2 = $"{{\"type\":\"Feature\",\"properties\":{{}},\"geometry\":{{\"type\":\"Point\",\"coordinates\":[{longitude},{lat}]}}}}";

        var www = UnityWebRequest.Put(postURL, geoJSONstring2);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("www error: " + www.error);
        }
    }

    IEnumerator ApplyDatasetToTileset()
    {
        string postURL = $"https://api.mapbox.com/uploads/v1/{m_myId}?access_token={m_accessToken}";

        string datasetTilesetInfo = $"{{\"tileset\":\"{m_myId}.{m_tilesetId}\",\"name\":\"{m_tilesetId}\",\"url\":\"mapbox://datasets/{m_myId}/{m_datasetId}\"}}";

        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(datasetTilesetInfo);

        var www = UnityWebRequest.Put(postURL, bytes);
        www.SetRequestHeader("content-type", "application/json");

        www.method = "POST"; // Hacked as Unity is weird.

        yield return www.SendWebRequest();
    }
}
