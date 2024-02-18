using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LitterHeatmapManager : MonoBehaviour
{
    private readonly Vector3 m_mapNormal = Vector3.down;

    [SerializeField] private AbstractMap m_map;
    [SerializeField] private MeshRenderer m_meshRenderer;

    [SerializeField] private LayerMask m_heatmapLayerMask;
    [SerializeField] private float m_maxDistance = 1000f;

    private Material m_material;
    private float[] m_points;

    private void Start()
    {
        m_material = m_meshRenderer.material;
    }

    private void Update()
    {
        List<LitterData> cachedLitter = LitterRecordingManager.Instance.CondensedLitterData;

        m_points = new float[3 * 32];
        int hitPointCount = 0;
        for (int i = 0; i < cachedLitter.Count; i++)
        {
            Vector2d location = Conversions.StringToLatLon(cachedLitter[i].Location);
            Vector3 worldPosition = m_map.GeoToWorldPosition(location, false);

            if (!IsInRange(worldPosition))
            {
                continue;
            }

            var ray = new Ray(worldPosition - m_mapNormal, m_mapNormal);
            if (Physics.Raycast(ray, out RaycastHit hit, 2f, m_heatmapLayerMask))
            {
                int index = hitPointCount * 3;
                m_points[index] = (hit.textureCoord.x * 4) - 2;
                m_points[index + 1] = (hit.textureCoord.y * 4) - 2;
                m_points[index + 2] = m_map.Zoom * 0.1f;

                hitPointCount++;
                hitPointCount %= 32;
            }
        }

        m_material.SetFloatArray("_Hits", m_points);
        m_material.SetInt("_HitCount", hitPointCount);
    }

    private bool IsInRange(Vector3 position)
    {
        return position.magnitude <= m_maxDistance;
    }

}
