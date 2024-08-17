using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class LitterHeatmapManager : MonoBehaviour
{
    private const int MAX_HIT_POINTS = 256;
    private readonly Vector3 m_mapNormal = Vector3.down;

    [SerializeField] private AbstractMap m_map;
    [SerializeField] private MeshRenderer m_meshRenderer;

    [SerializeField] private LayerMask m_heatmapLayerMask;
    [SerializeField] private float m_maxDistance = 1000f;

    private Material m_material;
    private float[] m_points;

    private int m_litterTimelineHours = 0;

    private void Start()
    {
        m_material = m_meshRenderer.material;

        m_meshRenderer.gameObject.SetActive(PlayerPrefs.GetInt(PrefsKeys.HEATMAP_ENABLED_KEY, 1) == 1);
        m_litterTimelineHours = SettingsPopup.TIMELINE_HOURS[PlayerPrefs.GetInt(PrefsKeys.LITTER_TIMELINE_KEY, 2)];
    }

    private void OnEnable()
    {
        SettingsPopup.OnHeatmapEnabledChanged += HandleHeatmapEnabledChanged;
        SettingsPopup.OnLitterTimelineChanged += OnLitterTimelineChanged;
    }

    private void OnDisable()
    {
        SettingsPopup.OnHeatmapEnabledChanged -= HandleHeatmapEnabledChanged;
        SettingsPopup.OnLitterTimelineChanged -= OnLitterTimelineChanged;
    }

    private void OnLitterTimelineChanged(int hours)
    {
        m_litterTimelineHours = hours;
    }

    private void Update()
    {
        List<LitterData> cachedLitter = LitterRecordingManager.Instance.FullLitterData;

        m_points = new float[3 * MAX_HIT_POINTS];
        int hitPointCount = 0;
        for (int i = 0; i < cachedLitter.Count; i++)
        {
            Vector2d location = Conversions.StringToLatLon(cachedLitter[i].Location);
            Vector3 worldPosition = m_map.GeoToWorldPosition(location, false);

            if (!IsInRange(worldPosition) || !IsInTimeline(cachedLitter[i].Timestamp))
            {
                continue;
            }

            var ray = new Ray(worldPosition - m_mapNormal, m_mapNormal);
            if (Physics.Raycast(ray, out RaycastHit hit, 2f, m_heatmapLayerMask))
            {
                if (hitPointCount < MAX_HIT_POINTS)
                {
                    int index = hitPointCount * 3;
                    m_points[index] = hit.textureCoord.x;
                    m_points[index + 1] = hit.textureCoord.y;
                    m_points[index + 2] = m_map.Zoom * 0.01f;

                    hitPointCount++;
                }
            }
        }

        m_material.SetFloatArray("_Hits", m_points);
        m_material.SetInt("_HitCount", hitPointCount);
    }

    private void HandleHeatmapEnabledChanged(bool enabled)
    {
        m_meshRenderer.gameObject.SetActive(enabled);
    }

    private bool IsInRange(Vector3 position)
    {
        return position.magnitude <= m_maxDistance;
    }

    private bool IsInTimeline(string timestamp)
    {
        if (DateTime.TryParse(timestamp, null, DateTimeStyles.AssumeUniversal, out var time))
        {
            return (DateTime.UtcNow - time).TotalHours <= m_litterTimelineHours;
        }

        return false;
    }
}
