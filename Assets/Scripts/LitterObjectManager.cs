using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System.Collections.Generic;
using UnityEngine;

public class LitterObjectManager : MonoBehaviour
{
    [SerializeField] private AbstractMap m_map;
    [SerializeField] private Transform m_litterObjectHolder;
    [SerializeField] private GameObject m_litterObjectPrefab;

    [SerializeField] private float m_maxDistance = 1000f;
    [SerializeField] private float m_MergedAmountScaleFactor = 0.5f;

    private List<GameObject> m_litterObjects = new List<GameObject>();

    private void Update()
    {
        List<LitterData> cachedLitter = LitterRecordingManager.Instance.CondensedLitterData;

        int objectCount = 0;
        for (int i = 0; i < cachedLitter.Count; i++)
        {
            Vector2d location = Conversions.StringToLatLon(cachedLitter[i].Location);
            Vector3 worldPosition = m_map.GeoToWorldPosition(location, false);

            if (!IsInRange(worldPosition))
            {
                continue;
            }

            if (objectCount >= m_litterObjects.Count)
            {
                SpawnNewLitterObject();
            }

            m_litterObjects[objectCount].SetActive(true);
            m_litterObjects[objectCount].transform.localPosition = worldPosition;
            m_litterObjects[objectCount].transform.localScale = Vector3.one + (Vector3.one * (cachedLitter[i].MergedAmount - 1) * m_MergedAmountScaleFactor);

            objectCount++;
        }

        for (; objectCount < m_litterObjects.Count; objectCount++)
        {
            m_litterObjects[objectCount].SetActive(false);
        }
    }

    private bool IsInRange(Vector3 position)
    {
        return position.magnitude <= m_maxDistance;
    }

    private void SpawnNewLitterObject()
    {
        GameObject newLitterObject = Instantiate(m_litterObjectPrefab, m_litterObjectHolder);
        newLitterObject.transform.localPosition = Vector3.zero;
        newLitterObject.transform.localScale = Vector3.one;
        m_litterObjects.Add(newLitterObject);
    }
}
