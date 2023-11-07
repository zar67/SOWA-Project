using Mapbox.Unity.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private AbstractMap m_abstractMap;

    private void Update()
    {
        if (GPS.Instance == null)
        {
            return;
        }

        m_abstractMap.SetCenterLatitudeLongitude(new Mapbox.Utils.Vector2d(GPS.Instance.Longitude, GPS.Instance.Latitude));
    }
}
