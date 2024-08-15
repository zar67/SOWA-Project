using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LitterObjectManager : MonoBehaviour
{
    [SerializeField] private AbstractMap m_map;
    [SerializeField] private Transform m_litterObjectHolder;
    [SerializeField] private LitterObject m_litterObjectPrefab;

    [SerializeField] private float m_maxDistance = 1000f;
    [SerializeField] private float m_mergedAmountScaleFactor = 0.5f;

    private bool m_locationPinsEnabled = true;

    private Camera m_uiCamera;
    private List<LitterObject> m_litterObjects = new List<LitterObject>();

    private void Awake()
    {
        m_uiCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
        m_locationPinsEnabled = PlayerPrefs.GetInt(PrefsKeys.LOCATION_PINS_ENABLED_KEY, 1) == 1;
    }

    private void OnEnable()
    {
        SettingsPopup.OnLocationPinsEnabledChanged += OnLocationPinsEnabledChanged;
    }

    private void OnDisable()
    {
        SettingsPopup.OnLocationPinsEnabledChanged -= OnLocationPinsEnabledChanged;
    }

    private void Update()
    {
        if (!m_locationPinsEnabled)
        {
            return;
        }

        List<List<LitterData>> cachedLitter = LitterRecordingManager.Instance.CondensedLitterData;

        int objectCount = 0;
        for (int i = 0; i < cachedLitter.Count; i++)
        {
            var location = new Vector2d();
            foreach (LitterData litter in cachedLitter[i])
            {
                location += Conversions.StringToLatLon(litter.Location);
            }

            location.x /= cachedLitter[i].Count;
            location.y /= cachedLitter[i].Count;

            Vector3 worldPosition = m_map.GeoToWorldPosition(location, false);

            if (!IsInRange(worldPosition))
            {
                continue;
            }

            if (objectCount >= m_litterObjects.Count)
            {
                SpawnNewLitterObject();
            }

            m_litterObjects[objectCount].gameObject.SetActive(true);
            m_litterObjects[objectCount].transform.localPosition = worldPosition;
            m_litterObjects[objectCount].SetData(cachedLitter[i]);

            objectCount++;
        }

        for (; objectCount < m_litterObjects.Count; objectCount++)
        {
            m_litterObjects[objectCount].gameObject.SetActive(false);
        }
    }

    private void OnLocationPinsEnabledChanged(bool enabled)
    {
        m_locationPinsEnabled = enabled;

        if (!m_locationPinsEnabled)
        {
            foreach (LitterObject obj in m_litterObjects)
            {
                obj.gameObject.SetActive(false);
            }
        }
    }

    private bool IsInRange(Vector3 position)
    {
        return position.magnitude <= m_maxDistance;
    }

    private void SpawnNewLitterObject()
    {
        LitterObject newLitterObject = Instantiate(m_litterObjectPrefab, m_litterObjectHolder);
        newLitterObject.transform.localPosition = Vector3.zero;
        newLitterObject.transform.localScale = Vector3.one;

        Canvas[] canvases = newLitterObject.GetComponentsInChildren<Canvas>(true);
        foreach (Canvas canvas in canvases)
        {
            canvas.worldCamera = m_uiCamera;
        }

        m_litterObjects.Add(newLitterObject);
    }
}
